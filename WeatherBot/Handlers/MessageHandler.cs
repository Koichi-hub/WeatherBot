using Telegram.Bot.Types;
using WeatherBot.Commands;

namespace WeatherBot.Handlers
{
    public class MessageHandler : IMessageHandler
    {
        private readonly IStartCommand startCommand;

        public MessageHandler(IStartCommand startCommand)
        {
            this.startCommand = startCommand;
        }

        public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            if (message.Text is not { } messageText)
                return;

            var command = messageText.Split(' ')[0] switch
            {
                "/start"    => startCommand.ExecuteAsync(message, cancellationToken),
                _           => Task.CompletedTask
            };

            await command;
        }
    }
}
