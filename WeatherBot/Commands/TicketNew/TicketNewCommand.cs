using Core.Constants;
using Core.Entities;
using Core.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherBot.Services;

namespace WeatherBot.Commands.TicketNew
{
    public class TicketNewCommand : ITicketNewCommand
    {
        private readonly ITelegramBotClient botClient;
        private readonly ISessionService sessionService;
        private readonly ITicketService ticketService;

        public TicketNewCommand(
            ITelegramBotClient botClient,
            ISessionService sessionService,
            ITicketService ticketService
        )
        {
            this.botClient = botClient;
            this.sessionService = sessionService;
            this.ticketService = ticketService;
        }

        public async Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken)
        {
            if (!session.IsAdmin)
                return;

            session.WaitResponseCommand = Core.Constants.Commands.TicketNew;
            await sessionService.UpdateAsync(session);

            var inlineKeyboard = new InlineKeyboardMarkup(WeatherTariffValues.AvailableWeatherTariffs.Select(x =>
                new[] { InlineKeyboardButton.WithCallbackData(text: x.ToString(), callbackData: ((int)x).ToString()) }
            ));

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Какой билет вы хотите выпустить?",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken
            );
        }

        public async Task HandleCallbackQueryResponse(Session session, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            if (!session.IsAdmin)
                return;

            session.WaitResponseCommand = string.Empty;
            await sessionService.UpdateAsync(session);

            if (
                int.TryParse(callbackQuery.Data, out int weatherTariff) &&
                Enum.IsDefined(typeof(WeatherTariff), weatherTariff)
            )
            {
                if (!await ticketService.CanIssueTicket((WeatherTariff)weatherTariff))
                {
                    await botClient.SendTextMessageAsync(
                        chatId: callbackQuery.From.Id,
                        text: $"Нельзя выпустить билет для тарифа {(WeatherTariff)weatherTariff}",
                        cancellationToken: cancellationToken
                    );
                    return;
                }

                var ticket = await ticketService.IssueTicket((WeatherTariff)weatherTariff);

                await botClient.SendTextMessageAsync(
                    chatId: callbackQuery.From.Id,
                    text: $"Выпущен новый билет `{ticket.Value}` для тарифа {ticket.WeatherTariff}",
                    parseMode: ParseMode.MarkdownV2,
                    cancellationToken: cancellationToken
                );
            }
        }
    }
}
