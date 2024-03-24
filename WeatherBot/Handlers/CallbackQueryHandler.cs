using Telegram.Bot.Types;
using WeatherBot.Commands.TicketNew;
using WeatherBot.Services;

namespace WeatherBot.Handlers
{
    public class CallbackQueryHandler : ICallbackQueryHandler
    {
        private readonly ISessionService sessionService;
        private readonly ITicketNewCommand ticketNewCommand;

        public CallbackQueryHandler(
            ISessionService sessionService,
            ITicketNewCommand ticketNewCommand
        )
        {
            this.sessionService = sessionService;
            this.ticketNewCommand = ticketNewCommand;
        }

        public async Task ExecuteAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            if (callbackQuery?.From?.Id == null)
                return;

            var session = await sessionService.GetAsync(callbackQuery.From.Id);

            if (session.WaitResponseCommand == null || session.UpdatedAt.Add(TimeSpan.FromMinutes(5)) < DateTime.UtcNow)
                return;

            var task = session.WaitResponseCommand switch
            {
                Core.Constants.Commands.TicketNew => ticketNewCommand.HandleCallbackQueryResponse(session, callbackQuery, cancellationToken),
                _ => Task.CompletedTask
            };
            await task;
        }
    }
}
