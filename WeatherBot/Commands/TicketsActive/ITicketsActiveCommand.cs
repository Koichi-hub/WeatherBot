using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Commands.TicketsActive
{
    public interface ITicketsActiveCommand
    {
        Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken);
    }
}
