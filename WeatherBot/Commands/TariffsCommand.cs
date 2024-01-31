using Core.Constants;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public class TariffsCommand : ITariffsCommand
    {
        private readonly ITelegramBotClient botClient;

        public TariffsCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
        }

        public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();
            sb.Append("Тарифы:\n");
            WeatherTariffValues.AvailableWeatherTariffs
                .ForEach(weatherTariff => sb.Append($"{weatherTariff} - {WeatherTariffValues.Limits[weatherTariff]} запросов\n"));

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: sb.ToString(),
                cancellationToken: cancellationToken
            );
        }
    }
}
