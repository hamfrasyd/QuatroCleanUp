namespace QuatroCleanUpBackend
{
    public class Event
    {
        public int EventId { get; set; } //unique EventId probably in the repo right?
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool FamilyFriendly { get; set; }
        public int Participants { get; set; }
        public byte[]? PictureId { get; set; }
        public decimal TrashCollected { get; set; }
        public int StatusId { get; set; }
        public int LocationId { get; set; }

        //Allright, so we don't always need a picture when creating an event
        public Event(string title, string description,
                     DateTime startTime, DateTime endTime,
                     bool familyFriendly, decimal trashCollected,
                     int statusId, int locationId)
        {
            Title = title;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            FamilyFriendly = familyFriendly;
            TrashCollected = trashCollected;
            StatusId = statusId;
            LocationId = locationId;
        }

        public override string ToString()
        {
            return $"The event's Id {EventId}." +
                $"The event's description is {Description}. " +
                $"The event's start time is {StartTime}." +
                $"The event's end time is {EndTime}." +
                $"The event is family friendly {FamilyFriendly}." +
                $"The event has collected {TrashCollected} kg of trash." +
                $"The event's status is {StatusId}." +
                $"The event's location is {LocationId}";
        }
    }
}
