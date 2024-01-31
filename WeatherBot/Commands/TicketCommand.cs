using Core.Constants;
using Core.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using WeatherBot.Services;

namespace WeatherBot.Commands
{
    public class TicketCommand : ITicketCommand
    {
        private readonly ITelegramBotClient botClient;
        private readonly ISessionService sessionService;
        private readonly ITicketService ticketService;

        public TicketCommand(
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
            session.WaitResponseCommand = Core.Constants.Commands.Ticket;
            await sessionService.UpdateAsync(session);

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Отправьте билет",
                cancellationToken: cancellationToken
            );
        }

        public async Task HandleResponse(Session session, Message message, CancellationToken cancellationToken)
        {
            session.WaitResponseCommand = string.Empty;
            await sessionService.UpdateAsync(session);

            var ticket = await ticketService.GetByValueAsyncOrDefault(message.Text!);
            if (ticket == null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Билет не найден :(",
                    cancellationToken: cancellationToken
                );
                return;
            }

            try
            {
                await ticketService.ActivateForSessionAsync(ticket, session);
            }
            catch (Exception)
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Не получилось активировать билет. Попробуйте ещё раз",
                    cancellationToken: cancellationToken
                );
                return;
            }

            if (session.WeatherTariff == Core.Enums.WeatherTariff.Admin)
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Теперь вы админ :)",
                    cancellationToken: cancellationToken
                );
                return;
            }

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Вы активировали билет!\nВаш тариф теперь - {session.WeatherTariff}, и вам доступно {WeatherTariffValues.Limits[session.WeatherTariff]} запросов",
                cancellationToken: cancellationToken
            );
        }
    }
}
