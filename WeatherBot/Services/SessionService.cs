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

        private async Task<Session> CreateSessionAsync(long userId)
        {
            var session = new Session
            {
                UserId = userId,
                WeatherTariff = WeatherTariff.None
            };

            return await sessionRepository.CreateAsync(session);
        }
    }
}
