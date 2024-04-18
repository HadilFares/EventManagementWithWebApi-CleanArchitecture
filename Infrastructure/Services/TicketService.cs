using Application.Interfaces.TicketRepository;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Services
{
    public class TicketService : BaseRepository<Ticket>, ITicketService
    {

        private readonly EventlyDbContext _context;
        public TicketService(EventlyDbContext context) : base(context)
        {

            _context = context;
        }


    }
}
