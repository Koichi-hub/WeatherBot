using Core.Entities;
using Core.Enums;

namespace DAL.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByValueAsyncOrDefault(string value);

        Task ActivateForSessionAsync(Ticket ticket, Session session);

        Task<Ticket> IssueTicket(WeatherTariff weatherTariff);

        Task<bool> CanIssueTicket(WeatherTariff weatherTariff);
    }
}
