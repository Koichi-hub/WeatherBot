using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public interface ITodayCommand
    {
        public Task ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
