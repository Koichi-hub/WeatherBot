using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Handlers
{
    public interface IMessageHandler
    {
        public Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken);
    }
}
