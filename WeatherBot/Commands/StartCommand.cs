using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public class StartCommand : IStartCommand
    {
        private readonly ITelegramBotClient botClient;

        public StartCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
        }

        public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id, 
                text: "Доброго дня! 👋.\nНа связи WeatherBot!",
                cancellationToken: cancellationToken
            );

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Чтобы узнать прогноз погоды на сегодня введите команду /today",
                cancellationToken: cancellationToken
            );
        }
    }
}
