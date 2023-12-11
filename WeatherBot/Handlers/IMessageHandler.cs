using Telegram.Bot.Types;

namespace WeatherBot.Handlers
{
    public interface IMessageHandler
    {
        public Task ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
