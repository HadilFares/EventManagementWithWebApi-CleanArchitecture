using Application.Interfaces.EventRepository;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Services
{
    public class EventService:BaseRepository<Event>,IEventService
    {
        private readonly EventlyDbContext _context;

        public EventService(EventlyDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Event>> GetAllNoValidatedEvents()
        {
            return await _context.Events.Where(e => !e.IsValidated).ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetAllValidatedEvents()
        {
            return await _context.Events.Where(e => e.IsValidated).ToListAsync();
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
    }
}
