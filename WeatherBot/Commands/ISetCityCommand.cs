using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public interface ISetCityCommand
    {
        Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken);

        Task HandleResponse(Session session, Message message, CancellationToken cancellationToken);
    }
}
