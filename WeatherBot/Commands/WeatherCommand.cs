using Core.Constants;
using Core.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using WeatherBot.Services;

namespace WeatherBot.Commands
{
    public class WeatherCommand : IWeatherCommand
    {
        private readonly ITelegramBotClient botClient;
        private readonly IWeatherService weatherService;
        private readonly ISessionService sessionService;

        public WeatherCommand(
            ITelegramBotClient botClient, 
            IWeatherService weatherService,
            ISessionService sessionService
        )
        {
            this.botClient = botClient;
            this.weatherService = weatherService;
            this.sessionService = sessionService;
        }

        public async Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken)
        {
            await sessionService.RefreshBalance(session);

            if (session.WeatherRequestCount <= 0)
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"У вас закончились запросы :(\nВы можете выбрать новый тариф, введите команду {Core.Constants.Commands.Tariffs}",
                    cancellationToken: cancellationToken
                );
                return;
            }

            if (session.City == null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Установите город",
                    cancellationToken: cancellationToken
                );
                return;
            }

            if (
                session.WeatherTariff == Core.Enums.WeatherTariff.Guest &&
                !await sessionService.CanGuestGetWeather(session)
            )
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "На сегодня кол-во гостей достигло максимума.\nПопробуйте завтра",
                    cancellationToken: cancellationToken
                );
                return;
            }

            try
            {
                var weather = await weatherService.GetWeather(session.City);

                var text = $"В городе {weather.City} ({weather.Country}) {weather.Status}.\nТемпература от {weather.Temperature.Min}°C до {weather.Temperature.Max}°C, ощущается как {weather.Temperature.FeelsLike}°C";

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: text,
                    cancellationToken: cancellationToken
                );

                session.WeatherRequestCount--;
                session.DateLastWeatherRequest = DateTime.UtcNow;
                await sessionService.UpdateAsync(session);
            }
            catch
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Упс. Во время выполнения команды {Core.Constants.Commands.Weather} возникла ошибка. Попробуйте ещё раз",
                    cancellationToken: cancellationToken
                );
            }
        }
    }
}
