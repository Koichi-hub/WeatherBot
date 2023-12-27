using Core.Entities;

namespace WeatherBot.Services
{
    public interface ISessionService
    {
        Task<Session> GetAsync(long userId);
    }
}
