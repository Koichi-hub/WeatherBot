using Core.Constants;
using Core.Entities;
using Core.Enums;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DatabaseContext databaseContext;
        private IQueryable<Ticket> AvailableTickets => databaseContext.Tickets.Where(t => !t.IsActivated);

        public TicketRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public Task<Ticket?> GetByValueAsyncOrDefault(string value)
        {
            return AvailableTickets.FirstOrDefaultAsync(t => t.Value == value);
        }

        public async Task ActivateForSessionAsync(Ticket ticket, Session session)
        {
            using var transaction = databaseContext.Database.BeginTransaction();

            ticket.IsActivated = true;
            ticket.Session = session;
            session.WeatherTariff = ticket.WeatherTariff;
            session.WeatherRequestCount = WeatherTariffValues.Limits[ticket.WeatherTariff];

            databaseContext.Sessions.Update(session);
            databaseContext.Tickets.Update(ticket);
            await databaseContext.SaveChangesAsync();

            transaction.Commit();
        }

        public async Task<Ticket> IssueTicket(WeatherTariff weatherTariff)
        {
            var ticket = new Ticket
            {
                WeatherTariff = weatherTariff,
                Value = TicketHelper.GenerateValue()
            };

            databaseContext.Tickets.Add(ticket);
            await databaseContext.SaveChangesAsync();

            return ticket;
        }

        public Task<bool> CanIssueTicket(WeatherTariff weatherTariff)
        {
            return weatherTariff switch
            {
                WeatherTariff.Client => CanIssueTicketForClient(),
                WeatherTariff.Vip => CanIssueTicketForVip(),
                _ => Task.FromResult(false),
            };   
        }

        private async Task<bool> CanIssueTicketForClient()
        {
            var count = await AvailableTickets.CountAsync(x => x.WeatherTariff == WeatherTariff.Client);
            return count < WeatherTariffValues.TotalClients;
        }

        private async Task<bool> CanIssueTicketForVip()
        {
            var count = await AvailableTickets.CountAsync(x => x.WeatherTariff == WeatherTariff.Vip);
            return count < WeatherTariffValues.TotalVips;
        }

        public async Task RemoveTicket(string value)
        {
            var ticket = await databaseContext.Tickets.FirstOrDefaultAsync(x => x.Value == value);
            if (ticket == null)
                return;
            databaseContext.Tickets.Remove(ticket);
            await databaseContext.SaveChangesAsync();
        }

        public Task<Ticket[]> GetActiveTickets()
        {
            return AvailableTickets.OrderByDescending(x => x.WeatherTariff).ToArrayAsync();
        }

        public Task<Ticket[]> GetActivatedTickets()
        {
            return databaseContext.Tickets
                .Include(x => x.Session)
                .Where(x => x.IsActivated)
                .OrderByDescending(x => x.WeatherTariff)
                .ToArrayAsync();
        }
    }
}
