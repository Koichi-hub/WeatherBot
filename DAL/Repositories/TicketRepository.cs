using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DatabaseContext databaseContext;
        private IQueryable<Ticket> AvailableTickets => databaseContext.Tickets.Where(t => t.IsActivated == false);

        public TicketRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public Task<Ticket?> GetByValueAsyncOrDefault(string value)
        {
            return AvailableTickets.FirstOrDefaultAsync(t => t.Value == value);
        }
    }
}
