using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public interface ITicketsActivatedCommand
    {
        Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken);
    }
}
