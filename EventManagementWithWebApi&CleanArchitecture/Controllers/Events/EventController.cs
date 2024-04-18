using Application.Dtos.Category;
using Application.Dtos.Event;
using Application.Interfaces.CategoryRepository;
using Application.Interfaces.EventRepository;
using Domain.Entities;
using Infra.Data.Identity.Roles;
using Infra.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace EventManagementWithWebApi_CleanArchitecture.Controllers.Events
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        IEventService _eventRepository;
        ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;
        public EventController(UserManager<User> userManager,IEventService eventRepository,ICategoryService categoryService)
        {
            _eventRepository = eventRepository;
            _categoryService= categoryService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllEvents")]
        //[Authorize(Roles = "Organizer")]

        public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
        {
            var events = await _eventRepository.GetAll();
            var eventsWithCategoryName = new List<EventDTO>();

            foreach (var e in events)
            {
                // Get the category name based on the category ID
                var categoryName = await _categoryService.GetCategoryById(e.CategoryId.Value);

                // Create the EventDTO and populate its properties
                var eventDto = new EventDTO
                { 
                    
                    Id=e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Type = e.Type,
                    Location = e.Location,
                    Price = e.Price,
                    NbStand = e.NbStand,
                    OrganizerId = e.UserId,
                    CategoryName = categoryName, // Assign the retrieved category name
                };

                // Add the EventDTO to the list
                eventsWithCategoryName.Add(eventDto);
            }

            return eventsWithCategoryName;
        }

        [HttpGet("validated")]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllValidatedEvents()
        {
            var events = await _eventRepository.GetAllValidatedEvents();
            

            return Ok(events);
        }

        [HttpGet("Notvalidated")]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllNotValidatedEvents()
        {
            var events = await _eventRepository.GetAllNoValidatedEvents();
            var eventsnoValidated = new List<EventDTO>();
            foreach (var e in events)
            {
                // Get the category name based on the category ID
                var categoryName = await _categoryService.GetCategoryById(e.CategoryId.Value);
                var Organizer = await _userManager.FindByIdAsync(e.UserId);

                // Create the EventDTO and populate its properties
                var eventDto = new EventDTO
                {

                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Type = e.Type,
                    Location = e.Location,
                    OrganizerName = Organizer.FirstName,
                    OrganizerLastName = Organizer.LastName,
                    CategoryName = categoryName, // Assign the retrieved category name
                };

                // Add the EventDTO to the list
                eventsnoValidated.Add(eventDto);
            }
            return eventsnoValidated;
        }

        [HttpPut("{id}/validate")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ValidateEvent(Guid id)
        {
            var existingEvent = await _eventRepository.Get(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            existingEvent.IsValidated = true;

            _eventRepository.Update(existingEvent);
            await _eventRepository.SaveChangesAsync();

            return NoContent();
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
            var newcategoryid = await _categoryService.GetCategoryByName(eventDto.CategoryName);

            existingEvent.Name = eventDto.Name;
            existingEvent.Description = eventDto.Description;
            existingEvent.Type = eventDto.Type;
            existingEvent.Price = eventDto.Price;
            existingEvent.CategoryId = newcategoryid;
            existingEvent.StartDate = eventDto.StartDate;
            existingEvent.EndDate = eventDto.EndDate;
            existingEvent.StartTime = eventDto.StartTime;
            existingEvent.EndTime = eventDto.EndTime;
            existingEvent.Location = eventDto.Location;
            existingEvent.NbStand = eventDto.NbStand;
            existingEvent.IsValidated = false;


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

            var categoryId = await _categoryService.GetCategoryByName(eventDto.CategoryName);
            var NewEvent = new Event
        {
            Name = eventDto.Name,
            UserId = eventDto.OrganizerId,
            Description = eventDto.Description,
            Type=eventDto.Type,
            Price=eventDto.Price,
            CategoryId= categoryId,
            StartDate=eventDto.StartDate,
            EndDate = eventDto.EndDate,
            StartTime = eventDto.StartTime,
            EndTime = eventDto.EndTime,
            Location = eventDto.Location,
            NbStand =eventDto.NbStand,
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


