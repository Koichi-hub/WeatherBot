using Telegram.Bot.Types;

namespace WeatherBot.Commands.Start
{
    public interface IStartCommand
    {
        public Task ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
