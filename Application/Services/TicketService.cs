using Application.Interfaces.IBaseRepository;
using Application.Interfaces.TicketRepository;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Infrastructure.Context;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly IBaseRepository<Ticket> _baseRepository;
        private readonly EventlyDbContext _context;
        public TicketService(EventlyDbContext context, IBaseRepository<Ticket> unitOfWork)
        {
            _baseRepository = unitOfWork;
            _context = context;
        }

        public async Task<Ticket> CreateTicket(Ticket ticket)
        {
            await  _baseRepository.Create(ticket);
            return ticket;
        }

        public async Task<bool> DeleteTicket(Guid id)
        {
            return await _baseRepository.Delete(id);
        }


        public async Task<Ticket> GetTicket(Guid id)
        {
            return await _baseRepository.Get(id);

        }

        public async Task<Ticket> GetTicketByEventId(Guid id)
        {
            return await _baseRepository.FindByConditionAsync(ticket => ticket.EventId == id);
        }

        public async Task<Ticket> UpdateTicket(Ticket ticket)
        {
             var x= await _baseRepository.Update(ticket);
            Console.WriteLine(x);
            return ticket;
        }
    }
}
