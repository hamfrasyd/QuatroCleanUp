using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuatroCleanUpBackend
{
    public class EventAttendance
    {

        public Event SomeEvent { get; set; }
        public User SomeUser { get; set; }
        public bool CheckIn { get; set; }
        public DateTime CreatedDate { get; set; }


        public EventAttendance(Event someEvent, User user, DateTime createdDate)
        {
            //if (someEvent == null) throw new ArgumentNullException(nameof(someEvent));
            //if (user == null) throw new ArgumentNullException(nameof(user));
            SomeEvent = someEvent;
            SomeUser = user;
            CreatedDate = createdDate;
            
        }

        public EventAttendance()
        {
            
        }

    }
}
