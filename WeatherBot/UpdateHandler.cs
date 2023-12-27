using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using WeatherBot.Handlers;
using WeatherBot.Services;

namespace WeatherBot;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> logger;
    private readonly IMessageHandler messageHandler;
    private readonly ISessionService sessionService;

    public UpdateHandler(
        ILogger<UpdateHandler> logger,
        IMessageHandler messageHandler,
        ISessionService sessionService
    )
    {
        this.logger = logger;
        this.messageHandler = messageHandler;
        this.sessionService = sessionService;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        if (update.Message?.From?.Id == null)
            return;

        var session = await sessionService.GetAsync(update.Message.From.Id);

        var handler = update switch
        {
            { Message: { } message } => messageHandler.ExecuteAsync(session, message, cancellationToken),
            _                        => Task.CompletedTask
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

        logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);

        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }
}
