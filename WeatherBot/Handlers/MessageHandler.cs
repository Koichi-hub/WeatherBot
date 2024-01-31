using Core.Entities;
using Telegram.Bot.Types;
using WeatherBot.Commands;

namespace WeatherBot.Handlers
{
    public class MessageHandler : IMessageHandler
    {
        private readonly IStartCommand startCommand;
        private readonly IWeatherCommand weatherCommand;
        private readonly ISetCityCommand setCityCommand;
        private readonly ICommandListCommand commandListCommand;
        private readonly ITariffsCommand tariffsCommand;
        private readonly ITicketCommand ticketCommand;
        private readonly IBalanceCommand balanceCommand;
        private readonly IContributingCommand contributingCommand;

        public MessageHandler(
            IStartCommand startCommand, 
            IWeatherCommand weatherCommand,
            ISetCityCommand setCityCommand,
            ICommandListCommand commandListCommand,
            ITariffsCommand tariffsCommand,
            ITicketCommand ticketCommand,
            IBalanceCommand balanceCommand,
            IContributingCommand contributingCommand
        )
        {
            this.startCommand = startCommand;
            this.weatherCommand = weatherCommand;
            this.setCityCommand = setCityCommand;
            this.commandListCommand = commandListCommand;
            this.tariffsCommand = tariffsCommand;
            this.ticketCommand = ticketCommand;
            this.balanceCommand = balanceCommand;
            this.contributingCommand = contributingCommand;
        }

        public Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken)
        {
            if (message.Text is not { } messageText)
                return Task.CompletedTask;

            return messageText.Split(' ')[0] switch
            {
                Core.Constants.Commands.Start => startCommand.ExecuteAsync(message, cancellationToken),
                Core.Constants.Commands.Ticket => ticketCommand.ExecuteAsync(session, message, cancellationToken),
                Core.Constants.Commands.Weather => weatherCommand.ExecuteAsync(session, message, cancellationToken),
                Core.Constants.Commands.Setcity => setCityCommand.ExecuteAsync(session, message, cancellationToken),
                Core.Constants.Commands.Balance => balanceCommand.ExecuteAsync(session, message, cancellationToken),
                Core.Constants.Commands.Tariffs => tariffsCommand.ExecuteAsync(message, cancellationToken),
                Core.Constants.Commands.Contributing => contributingCommand.ExecuteAsync(message, cancellationToken),
                Core.Constants.Commands.CommandList => commandListCommand.ExecuteAsync(message, cancellationToken),
                _ => HandleWaitResponseCommand(session, message, cancellationToken)
            };
        }

        private Task HandleWaitResponseCommand(Session session, Message message, CancellationToken cancellationToken)
        {
            if (session.WaitResponseCommand == null || session.UpdatedAt.Add(TimeSpan.FromMinutes(5)) < DateTime.UtcNow)
                return commandListCommand.ExecuteAsync(message, cancellationToken);

            return session.WaitResponseCommand switch
            {
                Core.Constants.Commands.Setcity => setCityCommand.HandleResponse(session, message, cancellationToken),
                Core.Constants.Commands.Ticket => ticketCommand.HandleResponse(session, message, cancellationToken),
                _ => commandListCommand.ExecuteAsync(message, cancellationToken)
            };
        }
    }
}
