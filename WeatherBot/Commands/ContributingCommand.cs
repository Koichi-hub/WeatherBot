using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public class ContributingCommand : IContributingCommand
    {
        private readonly ITelegramBotClient botClient;

        public ContributingCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
        }

        public Task ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            return botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Вы можете внести свой вклад в проект!\nСсылка на github: https://github.com/Koichi-hub/WeatherBot",
                cancellationToken: cancellationToken
            );
        }
    }
}
