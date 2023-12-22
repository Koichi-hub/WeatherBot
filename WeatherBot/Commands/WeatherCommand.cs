using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using WeatherBot.Services;
using WeatherBot.Settings;

namespace WeatherBot.Commands
{
    public class WeatherCommand : IWeatherCommand
    {
        private readonly ITelegramBotClient botClient;
        private readonly IWeatherService weatherService;
        private readonly AdminSettings adminSettings;

        public WeatherCommand(
            ITelegramBotClient botClient, 
            IWeatherService weatherService,
            IOptions<AdminSettings> adminSettings
        )
        {
            this.botClient = botClient;
            this.weatherService = weatherService;
            this.adminSettings = adminSettings.Value;
        }

        public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            if (message.From?.Id == adminSettings.Id)
            {

            }

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
