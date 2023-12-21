using Telegram.Bot;
using Telegram.Bot.Types;
using WeatherBot.Services;

namespace WeatherBot.Commands
{
    public class TodayCommand : ITodayCommand
    {
        private readonly ITelegramBotClient botClient;
        private readonly IWeatherService weatherService;

        public TodayCommand(ITelegramBotClient botClient, IWeatherService weatherService)
        {
            this.botClient = botClient;
            this.weatherService = weatherService;
        }

        public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var weather = await weatherService.GetWeather("perm");

            var text = $"В городе {weather.City} ({weather.Country}) {weather.Status}.\nТемпература от {weather.Temperature.Min}°C до {weather.Temperature.Max}°C, ощущается как {weather.Temperature.FeelsLike}°C";

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text,
                cancellationToken: cancellationToken
            );
        }
    }
}
