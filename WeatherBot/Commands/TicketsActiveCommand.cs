using Core.Entities;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherBot.Services;

namespace WeatherBot.Commands
{
    public class TicketsActiveCommand : ITicketsActiveCommand
    {
        private readonly ITelegramBotClient botClient;
        private readonly ITicketService ticketService;

        public TicketsActiveCommand(
            ITelegramBotClient botClient,
            ITicketService ticketService
        )
        {
            this.botClient = botClient;
            this.ticketService = ticketService;
        }

        public async Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken)
        {
            if (!session.IsAdmin)
                return;

            var activeTickets = await ticketService.GetActiveTickets();

            var sb = new StringBuilder();

            if (activeTickets.Length > 0)
            {
                sb.AppendLine("Активные билеты");
                sb.AppendLine("CreatedAt, Value, Tariff");
                foreach (var activeTicket in activeTickets)
                    sb.AppendLine($"{activeTicket.CreatedAt.ToString("d MMMM yyyy")}, `{activeTicket.Value}`, {activeTicket.WeatherTariff}");
            }
            else
                sb.AppendLine("Нет активных билетов");

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: sb.ToString(),
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken
            );
        }
    }
}
