using Core.Entities;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherBot.Services;

namespace WeatherBot.Commands
{
    public class TicketsActivatedCommand : ITicketsActivatedCommand
    {
        private readonly ITelegramBotClient botClient;
        private readonly ITicketService ticketService;

        public TicketsActivatedCommand(
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

            var activatedTickets = await ticketService.GetActivatedTickets();

            var sb = new StringBuilder();

            if (activatedTickets.Length > 0)
            {
                sb.AppendLine("Активированные билеты");
                sb.AppendLine("CreatedAt, UpdatedAt, UserId, Value, Tariff");
                foreach (var activatedTicket in activatedTickets)
                    sb.AppendLine($"{activatedTicket.CreatedAt.ToString("d MMMM yyyy")}, {activatedTicket.UpdatedAt.ToString("d MMMM yyyy")}, `{activatedTicket.Session!.UserId}`, `{activatedTicket.Value}`, {activatedTicket.WeatherTariff}");
            }
            else
                sb.AppendLine("Нет активированных билетов");

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: sb.ToString(),
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken
            );
        }
    }
}
