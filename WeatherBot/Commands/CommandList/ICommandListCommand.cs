using Telegram.Bot.Types;

namespace WeatherBot.Commands.CommandList
{
    public interface ICommandListCommand
    {
        Task ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
