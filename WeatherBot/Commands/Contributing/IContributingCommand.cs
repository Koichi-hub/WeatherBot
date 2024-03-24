using Telegram.Bot.Types;

namespace WeatherBot.Commands.Contributing
{
    public interface IContributingCommand
    {
        Task ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
