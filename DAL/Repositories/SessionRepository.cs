using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DatabaseContext databaseContext;

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
    }
}
