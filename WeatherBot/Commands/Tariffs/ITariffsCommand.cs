using Telegram.Bot.Types;

namespace WeatherBot.Commands.Tariffs
{
    public interface ITariffsCommand
    {
        public Task ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
