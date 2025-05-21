using QuatroCleanUpBackend;
using Microsoft.AspNetCore.Mvc;

namespace QuatroCleanUpApi.Controllers
{
    [Route("api/attendance")]

    [ApiController]
    public class AttendanceController : ControllerBase
    {
        //ControllerBase uses Microsoft.AspNetCore.Mvc;

        private readonly EventRepository _eventRepository;

        private readonly ILogger<EventController> _logger;

        public AttendanceController(EventRepository eventRepository, ILogger<EventController> logger) //Dependency Inject the repository class
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public IActionResult RecordAttendance([FromBody]int eventId){
            return Ok($"Attendance recorded for event {eventId}");
        }

       
            
    

    }
}
