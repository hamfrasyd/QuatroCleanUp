using QuatroCleanUpBackend;
using Microsoft.AspNetCore.Mvc;

namespace QuatroCleanUpApi.Controllers
{
    [Route("api/attendance")]

    [ApiController]
    public class AttendanceController : ControllerBase
    {

        public AttendanceController() //Dependency Inject the repository class
        {
        }

        public IActionResult RecordAttendance([FromBody]int eventId){
            return Ok($"Attendance recorded for event {eventId}");
        }

       
            
    

    }
}
