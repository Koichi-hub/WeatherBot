﻿using Core.Entities;
using Telegram.Bot.Types;

namespace WeatherBot.Commands
{
    public interface ITicketNewCommand
    {
        Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken);

        Task HandleCallbackQueryResponse(Session session, CallbackQuery callbackQuery, CancellationToken cancellationToken);
    }
}
