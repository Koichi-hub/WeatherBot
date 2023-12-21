using Microsoft.Extensions.Options;
using Telegram.Bot;
using WeatherBot;
using WeatherBot.Commands;
using WeatherBot.Handlers;
using WeatherBot.Services;
using WeatherBot.Settings;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<BotSettings>(context.Configuration.GetSection(BotSettings.SectionName));
        services.Configure<OpenWeatherSettings>(context.Configuration.GetSection(OpenWeatherSettings.SectionName));

        services.AddHttpClient("telegram_bot_client")
            .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
            {
                var botSettings = serviceProvider.GetService<IOptions<BotSettings>>()?.Value 
                    ?? throw new NullReferenceException(nameof(BotSettings));

                var options = new TelegramBotClientOptions(botSettings.BotToken);
                return new TelegramBotClient(options, httpClient);
            });

        services.AddHttpClient<IWeatherService, OpenWeatherService>();

        services.AddSingleton<IWeatherService, OpenWeatherService>();

        services.AddSingleton<IStartCommand, StartCommand>();
        services.AddSingleton<ITodayCommand, TodayCommand>();

        services.AddSingleton<IMessageHandler, MessageHandler>();

        services.AddSingleton<UpdateHandler>();
        services.AddHostedService<PoolingWorker>();
    })
    .Build();

await host.RunAsync();
