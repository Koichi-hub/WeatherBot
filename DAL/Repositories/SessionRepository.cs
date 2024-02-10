using Core.Constants;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DatabaseContext databaseContext;
        private IQueryable<Session> AvailableSessions => databaseContext.Sessions.Where(x => !x.IsBanned && !x.IsDeleted);

        public SessionRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<Session> CreateAsync(Session model)
        {
            var session = await databaseContext.Sessions.AddAsync(model);
            await databaseContext.SaveChangesAsync();

            return session.Entity;
        }

        public async Task<Session?> GetAsync(long userId)
        {
            return await databaseContext.Sessions.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<Session> UpdateAsync(Session session)
        {
            databaseContext.Sessions.Update(session);
            await databaseContext.SaveChangesAsync();
            return session;
        }

        public async Task<bool> CanGuestGetWeather(Session session)
        {
            var availableSessionsTodayPool = AvailableSessions.Where(x => x.DateLastWeatherRequest.Day == DateTime.UtcNow.Day);

            var count = await availableSessionsTodayPool.CountAsync(x => x.WeatherTariff == Core.Enums.WeatherTariff.Guest);

            if (count < WeatherTariffValues.TotalGuests)
                return true;

            return await availableSessionsTodayPool.AnyAsync(x => x.UserId == session.UserId);
        }
    }
}
