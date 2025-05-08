using QuatroCleanUpBackend;

namespace EventTests
{
    [TestClass]
    public sealed class EventRepositoryTest
    {
        [TestInitialize]
        public void Setup()
        {
            DateTime startTimeMock = new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc);
            DateTime endTimeMock = new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc);
            Event newEvent = new Event("Title", "Description",
                new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc),
                new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc),
                true, 10, 2, 5);
        }

        [TestMethod]
        public void CreateEventAsyncTest()
        {
            //Arrange
            Event newEvent = new Event("Title", "Description", 
                new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc),
                new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc),
                true,                10,                2,                5
                );
            EventRepository newEventRepo = new EventRepository();

            //Act
            var result = newEventRepo.CreateEventAsync(newEvent);

            //Assert
            Assert.AreEqual(result, newEvent);
        }


        [TestMethod]
        public void UpdateEventTest()
        {
            //Arrange - we need a created event and a new event, datawise.
            Event toBeUpdated = new Event("Title", "Description",
            new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc),
            new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc),
            true, 10, 2, 5  );

            Event eventUpdate = new Event("Title", "Description",
            new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc),
            new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc),
            true, 10, 2, 5);
            EventRepository newEventRepo = new EventRepository();

            //Act



        }




    }
}
