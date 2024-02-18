using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Handlers
{
    public interface IMessageHandler
    {
        Task ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
