using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Services.TicketService;

namespace WebAppMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _service;

        public TicketController(ITicketService service)
        {
            this._service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ticket>> GetTicket()
        {
            return Ok(_service.GetAllTickets() as List<Ticket>);
        }

        [Route("/api/ticket/getold/")]
        public ActionResult<IEnumerable<Ticket>> GetOldTickets()
        {
            return Ok(_service.GetAllOldTickets());
        }


        [Route("/api/ticket/getallsorted/")]
        public ActionResult<IEnumerable<Ticket>> GetTicketSorted()
        {
            return Ok(_service.GetAllTicketsSorted());
        }

        [Route("/api/ticket/getbysearch/{input}")]
        public ActionResult<IEnumerable<Ticket>> GetTicketSearch(string input)
        {
            return Ok(_service.GetAllTicketsSearch(input));
        }

        [Route("/api/ticket/countticket/")]
        public ActionResult<long> CountTicket()
        {
            return Ok(_service.CountThemAll());
        }

        [Route("/api/ticket/countticketopen/")]
        public ActionResult<IEnumerable<Ticket>> CountTicketOpen()
        {
            return Ok(_service.CountOpenTickets());
        }

        [Route("/api/ticket/countticketoverdue/")]
        public ActionResult<IEnumerable<Ticket>> CountTicketPverdue()
        {
            return Ok(_service.CountOverdueTickets());
        }

        [Route("/api/ticket/GetTicketById/{id}")]
        public ActionResult<Ticket> GetTicketById(string id)
        {
            Ticket ticket = _service.GetSingle(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        [HttpPost]
        [Route("/api/ticket/create/", Name = "Create Ticket")]
        public ActionResult<Ticket> CreateTicket(Ticket ticket)
        {

            if (ticket == null)
            {
                return BadRequest();
            }
            _service.AddTicket(ticket);

            return Ok(ticket);
        }

        [HttpPatch]
        [Route("/api/ticket/update/{id}", Name = "Update Ticket")]
        public IActionResult Update(string id, [FromBody] JsonPatchDocument<Ticket> patchDoc)
        {
            Ticket foundTicket = _service.GetSingle(id);
            patchDoc.ApplyTo(foundTicket);
            _service.UpdateTicket(foundTicket);

            return Ok();
        }

        [Route("/api/ticket/getemployeelist")]
        public ActionResult<IEnumerable<User>> GetEmployees()
        {
            return Ok(_service.GetAllEmployeesForTicket());
        }

        [Route("/api/ticket/deleteticket/{id}")]
        public IActionResult DeleteTicket(string id)
        {
            Ticket ticket = _service.GetSingle(id);

            if (ticket == null)
            {
                return NotFound();
            }

            _service.RemoveTicket(ticket);

            return Ok();
        }
    }
}
