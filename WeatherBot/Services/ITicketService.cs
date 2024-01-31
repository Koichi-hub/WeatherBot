using Core.Entities;

namespace WeatherBot.Services
{
    public interface ITicketService
    {
        Task ActivateForSessionAsync(Ticket ticket, Session session);

        Task<Ticket?> GetByValueAsyncOrDefault(string value);
    }
}
