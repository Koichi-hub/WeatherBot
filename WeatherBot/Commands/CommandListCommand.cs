using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public class CommandListCommand : ICommandListCommand
    {
        private readonly ITelegramBotClient botClient;

        public CommandListCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
        }

        public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();
            sb.Append("Список команд:\n");
            sb.Append($"{Core.Constants.Commands.Start}\n");
            sb.Append($"{Core.Constants.Commands.Weather} - прогноз погоды на сегодня\n");
            sb.Append($"{Core.Constants.Commands.Setcity} - задать город\n");
            sb.Append($"{Core.Constants.Commands.Balance} - доступное кол-во запросов\n");
            sb.Append($"{Core.Constants.Commands.Tariffs} - тарифы\n");
            sb.Append($"{Core.Constants.Commands.Contributing} - внести вклад в проект");

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: sb.ToString(),
                cancellationToken: cancellationToken
            );
        }
    }
}
