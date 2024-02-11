using Core.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using WeatherBot.Services;

namespace WeatherBot.Commands
{
    public class BalanceCommand : IBalanceCommand
    {
        private readonly ITelegramBotClient botClient;
        private readonly ISessionService sessionService;

        public BalanceCommand(
            ITelegramBotClient botClient,
            ISessionService sessionService
        )
        {
            this.botClient = botClient;
            this.sessionService = sessionService;
        }

        public async Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken)
        {
            await sessionService.RefreshBalance(session);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Баланс запросов: {session.WeatherRequestCount}",
                cancellationToken: cancellationToken
            );
        }
    }
}
