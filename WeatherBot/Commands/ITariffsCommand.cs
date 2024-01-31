using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public interface ITariffsCommand
    {
        public Task ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
