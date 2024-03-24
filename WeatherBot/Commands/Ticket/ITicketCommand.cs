using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Commands.Ticket
{
    public interface ITicketCommand
    {
        Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken);

        Task HandleResponse(Session session, Message message, CancellationToken cancellationToken);
    }
}
