using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.EventRepository
{
   public interface IEventService
    {
        Task<Event> GetEvent(Guid id);
        Task<Event> CreateEvent(Event e);
        Task<Event> UpdateEvent(Event e);
        Task<List<Event>> GetAllEvents();
        Task<bool> DeleteEvent(Guid id);
        Task<List<Event>> GetEventsByOrganizerId(string OrganizerId);
        Task<List<Event>> GetEventsByCatagoryId(Guid categoryId);
        Task<IEnumerable<Event>> GetAllValidatedEvents();
        Task<IEnumerable<Event>> GetAllNoValidatedEvents();
    }
}
