using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.EventRepository
{
   public interface IEventService:IBaseRepository<Event>
    {

        Task<List<Event>> GetEventsByOrganizerId(string OrganizerId);
        Task<List<Event>> GetEventsByCatagoryId(Guid categoryId);
        Task<IEnumerable<Event>> GetAllValidatedEvents();
    }
}
