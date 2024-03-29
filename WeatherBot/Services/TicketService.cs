﻿using Core.Entities;
using Core.Enums;
using DAL.Repositories;

namespace WeatherBot.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

        public Task<Ticket?> GetByValueAsyncOrDefault(string value)
        {
            return ticketRepository.GetByValueAsyncOrDefault(value);
        }

        public Task ActivateForSessionAsync(Ticket ticket, Session session)
        {
            return ticketRepository.ActivateForSessionAsync(ticket, session);
        }

        public Task<Ticket> IssueTicket(WeatherTariff weatherTariff)
        {
            return ticketRepository.IssueTicket(weatherTariff);
        }

        public Task<bool> CanIssueTicket(WeatherTariff weatherTariff)
        {
            return ticketRepository.CanIssueTicket(weatherTariff);
        }

        public Task RemoveTicket(string value)
        {
            return ticketRepository.RemoveTicket(value);
        }

        public Task<Ticket[]> GetActiveTickets()
        {
            return ticketRepository.GetActiveTickets();
        }

        public Task<Ticket[]> GetActivatedTickets()
        {
            return ticketRepository.GetActivatedTickets();
        }
    }
}
