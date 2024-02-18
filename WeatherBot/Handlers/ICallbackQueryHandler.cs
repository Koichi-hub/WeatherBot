using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Handlers
{
    public interface ICallbackQueryHandler
    {
        Task ExecuteAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken);
    }
}
