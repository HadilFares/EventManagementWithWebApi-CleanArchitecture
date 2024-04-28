using Application.Interfaces.EventRepository;
using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EventService :  IEventService
    {
        private readonly IBaseRepository<Event> _baseRepository;

        private readonly EventlyDbContext _context;

        public EventService(EventlyDbContext context, IBaseRepository<Event> baseRepository
            ) 
        {
            _context = context;
            _baseRepository = baseRepository;
        }

        public async Task<Event> CreateEvent(Event e)
        {

            _baseRepository.Create(e);
            await _baseRepository.SaveChangesAsync();
            return e;
        }

        public  async Task<bool> DeleteEvent(Guid id)
        {
            return await _baseRepository.Delete(id);
        }

        public async Task<List<Event>> GetAllEvents()
        {
           return  await _baseRepository.GetAll();
        }

        public async Task<IEnumerable<Event>> GetAllNoValidatedEvents()
        {
            return await _context.Events.Where(e => !e.IsValidated).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetAllValidatedEvents()
        {
            return await _context.Events.Where(e => e.IsValidated).ToListAsync();
        }

        public async Task<Event> GetEvent(Guid id)
        {
            
                return await _baseRepository.Get(id);
            
        }
        public async Task<List<Event>> GetEventsByCatagoryId(Guid categoryId)
        {
            return await _context.Events
                   .Where(c => c.CategoryId == categoryId)
                   .ToListAsync();
        }

        public async Task<List<Event>> GetEventsByOrganizerId(string OrganizerId)
        {
            return await _context.Events
                  .Where(c => c.UserId == OrganizerId)
                  .ToListAsync();
        }

        public async Task<Event> UpdateEvent(Event e)
        {
            _baseRepository.Update(e);
            await _baseRepository.SaveChangesAsync(); // Assuming SaveChangesAsync is implemented to save changes asynchronously
            return e;
        }
    }
}
