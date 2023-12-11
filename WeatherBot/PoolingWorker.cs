using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace WeatherBot;

public class PoolingWorker : BackgroundService
{
    private readonly ILogger<PoolingWorker> logger;
    private readonly ITelegramBotClient botClient;
    private readonly UpdateHandler updateHandler;

    public PoolingWorker(ILogger<PoolingWorker> logger, ITelegramBotClient botClient, UpdateHandler updateHandler)
    {
        this.logger = logger;
        this.botClient = botClient;
        this.updateHandler = updateHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await DoExecute(cancellationToken);
    }

    private async Task DoExecute(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var receiverOptions = new ReceiverOptions()
                {
                    AllowedUpdates = Array.Empty<UpdateType>(),
                    ThrowPendingUpdates = true,
                };

                var me = await botClient.GetMeAsync(cancellationToken);
                logger.LogInformation("Start receiving updates for {BotName}", me.Username ?? "My comrade");

                await botClient.ReceiveAsync(
                    updateHandler: updateHandler,
                    receiverOptions: receiverOptions,
                    cancellationToken: cancellationToken
                );
            }
            catch (Exception ex)
            {
                logger.LogError("Polling failed with exception: {Exception}", ex);
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }
}
