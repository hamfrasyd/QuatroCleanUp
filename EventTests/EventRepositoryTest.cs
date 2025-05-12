using Microsoft.Extensions.Logging;
using QuatroCleanUpBackend;

namespace EventTests
{
    [TestClass]
    public sealed class EventRepositoryTest
    {
        private Event testEvent;
        private Event testEvent2;
        private Event testEvent3;
        private Event testEvent4;


        [TestInitialize]
        public void Setup()
        {
            //DateTime startTimeMock = new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc);
            //DateTime endTimeMock = new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc);
            testEvent = new Event("Title", "Description",
                new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc),
                new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc),
                true, 10, 2, 5);
            Event newEvent = new Event("Title", "Description",
                new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc),
                new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc),
                true, 10, 2, 5);
        }

        [TestMethod]
        public void CreateEventAsyncTest()
        {
            //Arrange

            EventRepository newEventRepo = new EventRepository();

            //Act
            var result = newEventRepo.CreateEventAsync(testEvent);

            //Assert
            Assert.AreEqual(result, testEvent);
        }

        [TestMethod]
        public void CreateEventAsyncCreatesEventId()
        {
            //Arrange

            //Act

            //Assert
        }


        [TestMethod]
        public void UpdateEventTest()
        {
            //Arrange - we need a created event and a new event, datawise.
                //use testEvent - try to update that object.

            Event eventUpdate = new Event("Title", "Description",
            new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc),
            new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc),
            true, 10, 2, 5);
            EventRepository newEventRepo = new EventRepository();
            int eventId = 1;

            //Act
            testEvent.EventId = 1;
            Event result = newEventRepo.UpdateEventAsync(eventId, eventUpdate);
            result.EventId = 1;

            //Assert
            Assert.AreEqual(result, testEvent);

        }

        [TestMethod]
        public void TestEventIsDeletedFromDatabase()
        {
            //Arrange

            //Act

            //Assert

        }

        [TestMethod]
        public void TestGetAll()
        {
            //Arrange

            //Act

            //Assert
        }

        [TestMethod]
        public void TestGetById()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
