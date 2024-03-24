using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Commands.TicketRemove
{
    public interface ITicketRemoveCommand
    {
        Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken);

        Task HandleResponse(Session session, Message message, CancellationToken cancellationToken);
    }
}
