using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.TicketRepository
{
    public interface ITicketService: IBaseRepository<Ticket>
    {
    }
}
