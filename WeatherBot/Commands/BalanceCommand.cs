using Core.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public class BalanceCommand : IBalanceCommand
    {
        private readonly ITelegramBotClient botClient;

        public BalanceCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
        }

        public Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken)
        {
            return botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Баланс запросов: {session.WeatherRequestCount}",
                cancellationToken: cancellationToken
            );
        }
    }
}
