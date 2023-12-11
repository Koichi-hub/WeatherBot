using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public interface IStartCommand
    {
        public Task ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
