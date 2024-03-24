using DAL;
using DAL.Repositories;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using NLog.Extensions.Hosting;
using Telegram.Bot;
using WeatherBot;
using WeatherBot.Commands.Balance;
using WeatherBot.Commands.CommandList;
using WeatherBot.Commands.Contributing;
using WeatherBot.Commands.SetCity;
using WeatherBot.Commands.Start;
using WeatherBot.Commands.Tariffs;
using WeatherBot.Commands.Ticket;
using WeatherBot.Commands.TicketNew;
using WeatherBot.Commands.TicketRemove;
using WeatherBot.Commands.TicketsActivated;
using WeatherBot.Commands.TicketsActive;
using WeatherBot.Commands.Weather;
using WeatherBot.Handlers;
using WeatherBot.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder =>
    {
        builder.SetMinimumLevel(LogLevel.Trace);
        builder.ClearProviders();
    })
    .ConfigureServices((context, services) =>
    {
        //configs
        services.Configure<AppSettings>(context.Configuration.GetSection(AppSettings.SectionName));
        services.Configure<OpenWeatherSettings>(context.Configuration.GetSection(OpenWeatherSettings.SectionName));

        services.AddDbContext<DatabaseContext>();

        //http clients
        services.AddHttpClient("telegram_bot_client")
            .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
            {
                var appSettings = serviceProvider.GetService<IOptions<AppSettings>>()?.Value 
                    ?? throw new NullReferenceException(nameof(AppSettings));

                var options = new TelegramBotClientOptions(appSettings.BotToken);
                return new TelegramBotClient(options, httpClient);
            });

        services.AddHttpClient<IWeatherService, OpenWeatherService>();

        //repositories
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        //services
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IWeatherService, OpenWeatherService>();

        services.AddScoped<IStartCommand, StartCommand>();
        services.AddScoped<IWeatherCommand, WeatherCommand>();
        services.AddScoped<ISetCityCommand, SetCityCommand>();
        services.AddScoped<IBalanceCommand, BalanceCommand>();
        services.AddScoped<ICommandListCommand, CommandListCommand>();
        services.AddScoped<ITariffsCommand, TariffsCommand>();
        services.AddScoped<ITicketCommand, TicketCommand>();
        services.AddScoped<IContributingCommand, ContributingCommand>();

        services.AddScoped<ITicketNewCommand, TicketNewCommand>();
        services.AddScoped<ITicketRemoveCommand, TicketRemoveCommand>();
        services.AddScoped<ITicketsActiveCommand, TicketsActiveCommand>();
        services.AddScoped<ITicketsActivatedCommand, TicketsActivatedCommand>();

        //handlers
        services.AddScoped<IMessageHandler, MessageHandler>();
        services.AddScoped<ICallbackQueryHandler, CallbackQueryHandler>();

        services.AddScoped<UpdateHandler>();

        //hosted services
        services.AddHostedService<PoolingWorker>();
    })
    .UseNLog()
    .Build();

await host.RunAsync();
