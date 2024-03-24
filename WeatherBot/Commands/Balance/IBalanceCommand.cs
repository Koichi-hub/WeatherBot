using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Commands.Balance
{
    public interface IBalanceCommand
    {
        Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken);
    }
}
