using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public interface IWeatherCommand
    {
        public Task ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
