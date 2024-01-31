using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public interface IContributingCommand
    {
        Task ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
