using Core.Entities;

namespace DAL.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByValueAsyncOrDefault(string value);
    }
}
