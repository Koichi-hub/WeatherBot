using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace WeatherBot;

public class PoolingWorker : BackgroundService
{
    private readonly ILogger<PoolingWorker> logger;
    private readonly ITelegramBotClient botClient;
    private readonly IServiceProvider serviceProvider;

    public PoolingWorker(
        ILogger<PoolingWorker> logger, 
        ITelegramBotClient botClient,
        IServiceProvider serviceProvider
    )
    {
        this.logger = logger;
        this.botClient = botClient;
        this.serviceProvider = serviceProvider;
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
                using var scope = serviceProvider.CreateScope();
                var updateHandler = scope.ServiceProvider.GetService<IUpdateHandler>();

                var receiverOptions = new ReceiverOptions()
                {
                    AllowedUpdates = Array.Empty<UpdateType>(),
                    ThrowPendingUpdates = true,
                };

                var me = await botClient.GetMeAsync(cancellationToken);
                logger.LogInformation("Start receiving updates for {BotName}", me.Username ?? "My comrade");

                await botClient.ReceiveAsync(
                    updateHandler: updateHandler!,
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
