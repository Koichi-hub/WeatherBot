using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace WeatherBot;

public class PoolingWorker : BackgroundService
{
    private readonly static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    private readonly ITelegramBotClient botClient;
    private readonly IServiceProvider serviceProvider;

    public PoolingWorker(
        ITelegramBotClient botClient,
        IServiceProvider serviceProvider
    )
    {
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
                var updateHandler = scope.ServiceProvider.GetService<UpdateHandler>();

                var receiverOptions = new ReceiverOptions()
                {
                    AllowedUpdates = Array.Empty<UpdateType>(),
                    ThrowPendingUpdates = true,
                };

                var me = await botClient.GetMeAsync(cancellationToken);
                logger.Info("Start receiving updates for {BotName}", me.Username ?? "My comrade");

                await botClient.ReceiveAsync(
                    updateHandler: updateHandler!,
                    receiverOptions: receiverOptions,
                    cancellationToken: cancellationToken
                );
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Polling failed with exception: {Exception}");
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }
}
