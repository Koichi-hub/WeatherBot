using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using WeatherBot.Handlers;

namespace WeatherBot;

public class UpdateHandler : IUpdateHandler
{
    private readonly static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    private readonly IMessageHandler messageHandler;
    private readonly ICallbackQueryHandler callbackQueryHandler;

    public UpdateHandler(
        IMessageHandler messageHandler,
        ICallbackQueryHandler callbackQueryHandler
    )
    {
        this.messageHandler = messageHandler;
        this.callbackQueryHandler = callbackQueryHandler;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message } => messageHandler.ExecuteAsync(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => callbackQueryHandler.ExecuteAsync(callbackQuery, cancellationToken),
            _ => Task.CompletedTask
        };

        await handler;
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        logger.Error("HandleError: {ErrorMessage}", ErrorMessage);

        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }
}
