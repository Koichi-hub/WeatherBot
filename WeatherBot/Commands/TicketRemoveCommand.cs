using Core.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using WeatherBot.Services;

namespace WeatherBot.Commands
{
    public class TicketRemoveCommand : ITicketRemoveCommand
    {
        private readonly ITelegramBotClient botClient;
        private readonly ITicketService ticketService;
        private readonly ISessionService sessionService;

        public TicketRemoveCommand(
            ITelegramBotClient botClient,
            ITicketService ticketService,
            ISessionService sessionService
        )
        {
            this.botClient = botClient;
            this.ticketService = ticketService;
            this.sessionService = sessionService;
        }

        public async Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken)
        {
            if (!session.IsAdmin)
                return;

            session.WaitResponseCommand = Core.Constants.Commands.TicketRemove;
            await sessionService.UpdateAsync(session);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Отправьте билет, который хотите удалить",
                cancellationToken: cancellationToken
            );
        }

        public async Task HandleCallbackQueryResponse(Session session, Message message, CancellationToken cancellationToken)
        {
            if (!session.IsAdmin)
                return;

            session.WaitResponseCommand = string.Empty;
            await sessionService.UpdateAsync(session);

            await ticketService.RemoveTicket(message.Text!);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Билет {message.Text} был удален",
                cancellationToken: cancellationToken
            );
        }
    }
}
