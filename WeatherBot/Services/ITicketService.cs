using Core.Entities;
using Core.Enums;

namespace WeatherBot.Services
{
    public interface ITicketService
    {
        Task<Ticket?> GetByValueAsyncOrDefault(string value);

        Task ActivateForSessionAsync(Ticket ticket, Session session);

        Task<Ticket> IssueTicket(WeatherTariff weatherTariff);

        Task<bool> CanIssueTicket(WeatherTariff weatherTariff);

        Task<Ticket[]> GetActiveTickets();

        Task<Ticket[]> GetActivatedTickets();
    }
}
