using Core.Constants;
using Core.Entities;
using DAL;
using DAL.Repositories;

namespace WeatherBot.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository ticketRepository;
        private readonly DatabaseContext databaseContext;

        public TicketService(
            ITicketRepository ticketRepository,
            DatabaseContext databaseContext
        )
        {
            this.ticketRepository = ticketRepository;
            this.databaseContext = databaseContext;
        }

        public Task<Ticket?> GetByValueAsyncOrDefault(string value)
        {
            return ticketRepository.GetByValueAsyncOrDefault(value);
        }

        public async Task ActivateForSessionAsync(Ticket ticket, Session session)
        {
            using var transaction = databaseContext.Database.BeginTransaction();

            ticket.IsActivated = true;
            ticket.Session = session;
            session.WeatherTariff = ticket.WeatherTariff;
            if (ticket.WeatherTariff != Core.Enums.WeatherTariff.Admin)
                session.WeatherRequestCount = WeatherTariffValues.Limits[ticket.WeatherTariff];

            databaseContext.Sessions.Update(session);
            databaseContext.Tickets.Update(ticket);
            await databaseContext.SaveChangesAsync();

            transaction.Commit();
        }
    }
}
