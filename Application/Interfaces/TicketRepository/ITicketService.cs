using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.TicketRepository
{
    public interface ITicketService
    {
       Task<Ticket> GetTicket(Guid id);
      Task<Ticket> GetTicketByEventId(Guid eventId);
        Task<Ticket> CreateTicket(Ticket ticket);
        Task<Ticket>    UpdateTicket(Ticket ticket);
        Task<bool> DeleteTicket(Guid id);

    }
}
