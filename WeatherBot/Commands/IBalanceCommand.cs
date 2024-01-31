using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public interface IBalanceCommand
    {
        Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken);
    }
}
