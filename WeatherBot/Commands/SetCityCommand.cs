using Core.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using WeatherBot.Services;

namespace WeatherBot.Commands
{
    public class SetCityCommand : ISetCityCommand
    {
        private readonly ITelegramBotClient botClient;
        private readonly ISessionService sessionService;

        public SetCityCommand(
            ITelegramBotClient botClient,
            ISessionService sessionService
        )
        {
            this.botClient = botClient;
            this.sessionService = sessionService;
        }

        public async Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken)
        {
            session.WaitResponseCommand = Core.Constants.Commands.Setcity;
            await sessionService.UpdateAsync(session);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Отправьте название города",
                cancellationToken: cancellationToken
            );
        }

        public async Task HandleResponse(Session session, Message message, CancellationToken cancellationToken)
        {
            session.City = message.Text;
            await sessionService.UpdateAsync(session);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Название города установлено {session.City}, теперь вы можете выполнить команду {Core.Constants.Commands.Weather}",
                cancellationToken: cancellationToken
            );
        }
    }
}
