using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Commands.Weather
{
    public interface IWeatherCommand
    {
        public Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken);
    }
}
