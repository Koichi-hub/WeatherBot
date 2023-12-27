using Core.Entities;

namespace DAL.Repositories
{
    public interface ISessionRepository
    {
        Task<Session?> GetAsync(long userId);

        Task<Session> CreateAsync(Session model);
    }
}
