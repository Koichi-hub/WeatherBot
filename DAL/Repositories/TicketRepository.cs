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
        private IQueryable<Ticket> AvailableTickets => databaseContext.Tickets.Where(t => t.IsActivated == false);
        private IQueryable<Session> AvailableSessions => databaseContext.Sessions.Where(x => !x.IsBanned && !x.IsDeleted);

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

        public async Task<Ticket?> IssueTicket(WeatherTariff weatherTariff)
        {
            if (!await CanIssueTicket(weatherTariff))
                return null;

            var ticket = new Ticket
            {
                WeatherTariff = weatherTariff,
                Value = TicketHelper.GenerateValue()
            };

            databaseContext.Tickets.Add(ticket);
            await databaseContext.SaveChangesAsync();

            return ticket;
        }

        private Task<bool> CanIssueTicket(WeatherTariff weatherTariff)
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
            var count = await AvailableSessions.CountAsync(x => x.WeatherTariff == WeatherTariff.Client);
            return count < WeatherTariffValues.TotalClients;
        }

        private async Task<bool> CanIssueTicketForVip()
        {
            var count = await AvailableSessions.CountAsync(x => x.WeatherTariff == WeatherTariff.Vip);
            return count < WeatherTariffValues.TotalVips;
        }
    }
}
