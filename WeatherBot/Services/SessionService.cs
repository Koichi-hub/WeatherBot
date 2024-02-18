using Core.Constants;
using Core.Entities;
using Core.Enums;
using DAL.Repositories;

namespace WeatherBot.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository;
        }

        public async Task<Session> GetAsync(long userId)
        {
            var session = await sessionRepository.GetAsync(userId);

            if (session == null)
                return await CreateSessionAsync(userId);

            return session;
        }

        private Task<Session> CreateSessionAsync(long userId)
        {
            var session = new Session
            {
                UserId = userId,
                WeatherTariff = WeatherTariff.Guest,
                WeatherRequestCount = WeatherTariffValues.Limits[WeatherTariff.Guest],
                DateLastWeatherRequest = DateTime.UtcNow.AddDays(-1)
            };

            return sessionRepository.CreateAsync(session);
        }

        public Task<Session> UpdateAsync(Session session)
        {
            return sessionRepository.UpdateAsync(session);
        }

        public Task<bool> CanGuestGetWeather(Session session)
        {
            return sessionRepository.CanGuestGetWeather(session);
        }

        public Task RefreshBalance(Session session)
        {
            if (session.DateLastWeatherRequest.Day < DateTime.UtcNow.Day)
            {
                session.WeatherRequestCount = WeatherTariffValues.Limits[session.WeatherTariff];
                session.DateLastWeatherRequest = DateTime.UtcNow;
                return sessionRepository.UpdateAsync(session);
            }
            return Task.CompletedTask;
        }
    }
}
