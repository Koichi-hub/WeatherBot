﻿using Core.Entities;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using WeatherBot.Services;

namespace WeatherBot.Commands
{
    public class WeatherCommand : IWeatherCommand
    {
        private readonly ITelegramBotClient botClient;
        private readonly IWeatherService weatherService;
        private readonly AppSettings appSettings;

        public WeatherCommand(
            ITelegramBotClient botClient, 
            IWeatherService weatherService,
            IOptions<AppSettings> appSettings
        )
        {
            this.botClient = botClient;
            this.weatherService = weatherService;
            this.appSettings = appSettings.Value;
        }

        public async Task ExecuteAsync(Session session, Message message, CancellationToken cancellationToken)
        {
            if (message.From?.Id == appSettings.AdminId)
            {

            }

            if (session.City == null)
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Установите город",
                    cancellationToken: cancellationToken
                );
                return;
            }

            var weather = await weatherService.GetWeather(session.City);

            var text = $"В городе {weather.City} ({weather.Country}) {weather.Status}.\nТемпература от {weather.Temperature.Min}°C до {weather.Temperature.Max}°C, ощущается как {weather.Temperature.FeelsLike}°C";

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text,
                cancellationToken: cancellationToken
            );
        }
    }
}