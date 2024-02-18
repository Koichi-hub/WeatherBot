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
            sb.AppendLine("Список команд:");
            sb.AppendLine($"{Core.Constants.Commands.Start}");
            sb.AppendLine($"{Core.Constants.Commands.Ticket} - использовать билет для активации тарифа");
            sb.AppendLine($"{Core.Constants.Commands.Weather} - прогноз погоды на сегодня");
            sb.AppendLine($"{Core.Constants.Commands.Setcity} - задать город");
            sb.AppendLine($"{Core.Constants.Commands.Balance} - доступное кол-во запросов");
            sb.AppendLine($"{Core.Constants.Commands.Tariffs} - тарифы");
            sb.AppendLine($"{Core.Constants.Commands.Contributing} - внести вклад в проект");

            //admin
            sb.AppendLine($"{Core.Constants.Commands.TicketNew} - выпустить билет");
            sb.AppendLine($"{Core.Constants.Commands.TicketRemove} - удалить билет");
            sb.AppendLine($"{Core.Constants.Commands.TicketsActive} - список активных билетов");
            sb.AppendLine($"{Core.Constants.Commands.TicketsActivated} - список использованных билетов");

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: sb.ToString(),
                cancellationToken: cancellationToken
            );
        }
    }
}
