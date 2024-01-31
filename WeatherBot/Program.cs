using DAL;
using DAL.Repositories;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using WeatherBot;
using WeatherBot.Commands;
using WeatherBot.Handlers;
using WeatherBot.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<AppSettings>(context.Configuration.GetSection(AppSettings.SectionName));
        services.Configure<OpenWeatherSettings>(context.Configuration.GetSection(OpenWeatherSettings.SectionName));

        services.AddDbContext<DatabaseContext>();

        services.AddHttpClient("telegram_bot_client")
            .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
            {
                var appSettings = serviceProvider.GetService<IOptions<AppSettings>>()?.Value 
                    ?? throw new NullReferenceException(nameof(AppSettings));

                var options = new TelegramBotClientOptions(appSettings.BotToken);
                return new TelegramBotClient(options, httpClient);
            });

        services.AddHttpClient<IWeatherService, OpenWeatherService>();

        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IWeatherService, OpenWeatherService>();

        services.AddScoped<IStartCommand, StartCommand>();
        services.AddScoped<IWeatherCommand, WeatherCommand>();
        services.AddScoped<ISetCityCommand, SetCityCommand>();
        services.AddScoped<ICommandListCommand, CommandListCommand>();
        services.AddScoped<ITariffsCommand, TariffsCommand>();
        services.AddScoped<ITicketCommand, TicketCommand>();

        services.AddScoped<IMessageHandler, MessageHandler>();

        services.AddScoped<UpdateHandler>();
        services.AddHostedService<PoolingWorker>();
    })
    .Build();

await host.RunAsync();
