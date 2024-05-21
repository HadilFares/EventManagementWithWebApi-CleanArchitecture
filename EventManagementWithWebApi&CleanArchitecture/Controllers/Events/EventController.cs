using Application.Dtos.Category;
using Application.Dtos.Event;
using Application.Interfaces.CategoryRepository;
using Application.Interfaces.EventRepository;
using Domain.Entities;
using Infra.Data.Identity.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace EventManagementWithWebApi_CleanArchitecture.Controllers.Events
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {

        private readonly ILogger<EventController> _logger;
        IEventService _eventRepository;
        ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;
        public EventController(UserManager<User> userManager,IEventService eventRepository,ICategoryService categoryService, ILogger<EventController> logger)
        {
            _eventRepository = eventRepository;
            _categoryService= categoryService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllEvents")]
        //[Authorize(Roles = "Organizer")]

        public async Task<ActionResult<IEnumerable<EventAllDTO>>> GetAllEvents()
        {
            var events = await _eventRepository.GetAllEvents();
            var eventsWithCategoryName = new List<EventAllDTO>();

            foreach (var e in events)
            {
                // Get the category name based on the category ID
                var categoryName = await _categoryService.GetCategoryById(e.CategoryId.Value);

                // Create the EventDTO and populate its properties
                var eventDto = new EventAllDTO
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
                    CategoryName = categoryName,
                    Photo=e.Photo

                    // Assign the retrieved category name
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
            var existingEvent = await _eventRepository.GetEvent(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            existingEvent.IsValidated = true;

            _eventRepository.UpdateEvent(existingEvent);

            return NoContent();
        }



        [HttpGet]
        [Route("GetEventById/{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var course = await _eventRepository.GetEvent(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var deleted = await _eventRepository.DeleteEvent(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromForm] EventDTO eventDto)
        {

            var existingEvent = await _eventRepository.GetEvent(id);
            if (existingEvent == null)
            {
                return NotFound();
            }
            var newcategoryid = await _categoryService.GetCategoryByName(eventDto.CategoryName);
            string savedImagePath = null;
            if (eventDto.Photo != null && eventDto.Photo.Length > 0)
            {
                savedImagePath = await _eventRepository.SaveImage(eventDto.Photo);
                if (savedImagePath == null)
                {
                    return BadRequest("Invalid image file.");
                }
            }
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
            existingEvent.Photo = savedImagePath;
            existingEvent.IsValidated = false;


          await   _eventRepository.UpdateEvent(existingEvent);

         

            return Ok(existingEvent);

        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromForm] EventDTO eventDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryId = await _categoryService.GetCategoryByName(eventDto.CategoryName);

            string savedImagePath = null;
            if (eventDto.Photo != null && eventDto.Photo.Length > 0)
            {
                savedImagePath =  await _eventRepository.SaveImage(eventDto.Photo);
                if (savedImagePath == null)
                {
                    return BadRequest("Invalid image file.");
                }
            }

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
            Photo= savedImagePath

            };

   await  _eventRepository.CreateEvent(NewEvent);

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> FilterEvents ([FromQuery] EventFilter searchFilter)
        {
            var events = await _eventRepository.GetAllEvents();
            var categoryId = await _categoryService.GetCategoryByName(searchFilter.CategoryName);

            if (!string.IsNullOrEmpty(searchFilter.CategoryName))
            {
                events = (List<Event>)events.Where(e => e.CategoryId==categoryId);
            }

            if (!string.IsNullOrEmpty(searchFilter.Location))
            {
                events = (List<Event>)events.Where(e => e.Location == searchFilter.Location);
            }

            return Ok(events);
        }


        /* [HttpPost("save-image")]
         public async Task<IActionResult> SaveImage(IFormFile photo)
         {
             if (photo == null || photo.Length == 0)
             {
                 return BadRequest("No file was provided.");
             }

             var fileName = Path.GetFileName(photo.FileName);
             var basePath = @"D:\photoevent";
             var filePath = Path.Combine(basePath, fileName);

             using (var stream = new FileStream(filePath, FileMode.Create))
             {
                 await photo.CopyToAsync(stream);
             }


             return Ok(new { message = "Image saved successfully." });
         }
        */
        /*[HttpPost("save-image")]
        public async Task<string> SaveImage(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                return BadRequest("No file was provided.");
            }

            // Get the file name and sanitize it
            var fileName = Path.GetFileName(photo.FileName);
            var sanitizedFileName = SanitizeFileName(fileName);

            var basePath = @"D:\photoevent";
            var filePath = Path.Combine(basePath, sanitizedFileName);

            // Ensure the directory exists
            Directory.CreateDirectory(basePath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            return Ok(new { message = "Image saved successfully." });
        }


        private string SanitizeFileName(string fileName)
        {
            // Replace spaces with underscores
            var sanitizedFileName = fileName.Replace(" ", "_");

            // Further sanitize by removing any invalid characters
            sanitizedFileName = string.Concat(sanitizedFileName.Split(Path.GetInvalidFileNameChars()));

            return sanitizedFileName;
        }*/

    }
}


