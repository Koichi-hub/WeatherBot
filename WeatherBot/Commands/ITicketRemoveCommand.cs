using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public interface ITicketRemoveCommand
    {
        Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken);

        Task HandleCallbackQueryResponse(Session session, Message message, CancellationToken cancellationToken);
    }
}
