using QuatroCleanUpBackend;
using Microsoft.AspNetCore.Mvc;

namespace QuatroCleanUpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        //ControllerBase uses Microsoft.AspNetCore.Mvc;

        private readonly EventRepository _eventRepository;

        public EventController(EventRepository eventRepository) //Dependency Inject the repository class
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get()
        {
            var eventList = await _eventRepository.GetAllAsync();

            if(eventList.Count == 0 || eventList is null)
            {
                return NoContent();
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var e = await _eventRepository.GetByIdAsync(id);
                return Ok(e);
            }
            catch (KeyNotFoundException KeyNotFoundException)
            {
                return NotFound();
            }
        }


        //[HttpGet]
        //[Route("SortByDateAscending")]
        //Er det en metode vi vil have?

        //[HttpGet]
        //[Route("SortByDateDescending")]
        //Er det en metode vi vil have?

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Event newEvent)
        {
            try
            {
                Event e = await _eventRepository.CreateEventAsync(newEvent);
                return Created("api/Events" + e.EventId, e);
            }
            catch(ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
        }


        [HttpPut]
        [Route("{EventId}")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, Event eventUpdate)
        {
            try
            {
                Event newEventUpdate = await _eventRepository.UpdateEventAsync(eventUpdate);
                return Ok(newEventUpdate);
            }
            catch(ArgumentException argumentE)
            {
                return BadRequest();
            }
            catch(KeyNotFoundException keyNotFoundExeption)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var returnEvent = await _eventRepository.DeleteEventAsync(id);
                return Ok(returnEvent);
            }
            catch(KeyNotFoundException keyNotFoundException)
            {
                return NotFound();
            }
        }


    }
}
