using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public interface ICommandListCommand
    {
        Task ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
