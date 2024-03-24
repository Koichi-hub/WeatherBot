using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Commands.TicketNew
{
    public interface ITicketNewCommand
    {
        Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken);

        Task HandleCallbackQueryResponse(Session session, CallbackQuery callbackQuery, CancellationToken cancellationToken);
    }
}
