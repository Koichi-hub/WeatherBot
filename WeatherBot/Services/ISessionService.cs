using Core.Entities;

namespace WeatherBot.Services
{
    public interface ISessionService
    {
        Task<Session> GetAsync(long userId);

        Task<Session> UpdateAsync(Session session);

        Task<bool> CanGuestGetWeather(Session session);

        Task RefreshBalance(Session session);
    }
}
