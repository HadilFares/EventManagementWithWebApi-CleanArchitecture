using Application.Dtos.Ticket;
using Application.Interfaces.EventRepository;
using Application.Interfaces.TicketRepository;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementWithWebApi_CleanArchitecture.Controllers.Tickets
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private ITicketService _ticketservice;
        private IEventService _eventService;
        public TicketController(ITicketService ticketservice, IEventService eventService)
        {
            _ticketservice = ticketservice;
            _eventService = eventService;

        }

        [HttpGet]
        [Route("GetTicketByEventId/{id}")]
        //[Authorize]
        public async Task<IActionResult> GetTicketByEventId(Guid id)
        {
            var ticket = await _ticketservice.FindByConditionAsync(ticket => ticket.EventId == id);
            if (ticket != null)
            {
                return Ok(ticket);
            }
            return NotFound();
        }


        [HttpGet]
        [Route("GetTicket/{id}")]
        //[Authorize]
        public async Task<IActionResult> GetTicket(Guid id)
        {
            var ticket = await _ticketservice.Get(id);
            if (ticket != null)
            {
                return Ok(ticket);
            }
            return NotFound();
        }
        [HttpPost]
        //[Authorize()]
        public async Task<ActionResult> CreateTicket([FromBody] TicketDTO ticketDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // var Event = await _eventService.Get(ticketDTO.EventId);



            var ticket = new Ticket
            {
                Name = ticketDTO.Name,
                EventId = ticketDTO.EventId,
                EndDate = ticketDTO.EndDate,
                StartTime = ticketDTO.StartTime,
                EndTime = ticketDTO.EndTime,
                Location = ticketDTO.Location,
                Price = ticketDTO.Price,
                StartDate = ticketDTO.StartDate,
                Ticketcolor=ticketDTO.TicketColor

            };

            _ticketservice.Create(ticket);
            await _ticketservice.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var deleted = await _ticketservice.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

}
