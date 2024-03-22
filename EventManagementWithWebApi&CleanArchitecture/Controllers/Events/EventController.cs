using Application.Dtos.Category;
using Application.Dtos.Event;
using Application.Interfaces.CategoryRepository;
using Application.Interfaces.EventRepository;
using Domain.Entities;
using Infra.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementWithWebApi_CleanArchitecture.Controllers.Events
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        IEventService _eventRepository;
        public EventController(IEventService eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        [Route("GetAllEvents")]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            var events = await _eventRepository.GetAll();
            return events;
        }

        [HttpGet]
        [Route("GetEventById/{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var course = await _eventRepository.Get(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var deleted = await _eventRepository.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventDTO eventDto)
        {

            var existingEvent = await _eventRepository.Get(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            existingEvent.Name = eventDto.Name;


            _eventRepository.Update(existingEvent);
            await _eventRepository.SaveChangesAsync();

            return NoContent();

        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventDTO eventDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var NewEvent = new Event
        {
            Name = eventDto.Name,
            OrganizerId = eventDto.OrganizerId,
            Description = eventDto.Description,
            Type=eventDto.Type,
            Price=eventDto.Price,
            CategoryId=eventDto.CategoryId,
            Date=eventDto.Date,
            Location = eventDto.Location,
            NbStand =eventDto.NbStand,
            Ratings= eventDto.Ratings
    };

    _eventRepository.Create(NewEvent);
    await _eventRepository.SaveChangesAsync();
      return CreatedAtAction(nameof(GetEventById), new { id = NewEvent.Id }, NewEvent);


    }


        [HttpGet("GetEventsByOrganizerId/{OrganizerId}")]
        public async Task<IActionResult> GetEventsByOrganizerId(string OrganizerId)
        {
            var events = await _eventRepository.GetEventsByOrganizerId(OrganizerId);
            if (events == null || !events.Any())
            {
                return NotFound();
            }

            return Ok(events);
        }

        [HttpGet("GetEventsBycategoryId/{categoryId}")]
        public async Task<IActionResult> GetEventsByCatagoryId(Guid categoryId)
        {
            var events = await _eventRepository.GetEventsByCatagoryId(categoryId);
            if (events == null || !events.Any())
            {
                return NotFound();
            }

            return Ok(events);
        }
    }
}


