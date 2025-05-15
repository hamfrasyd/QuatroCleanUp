using QuatroCleanUpBackend;

namespace EventTests
{
    //[TestClass]
    //public sealed class EventRepositoryTest
    //{
    //    private Event testEvent;
    //    private Event testEvent2;
    //    private Event testEvent3;
    //    private Event testEvent4;
    //    private List<Event> testList;


    //    [TestInitialize]
    //    public void Setup()
    //    {
    //        //DateTime startTimeMock = new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc);
    //        //DateTime endTimeMock = new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc);
    //        testEvent = new Event("Title", "Description",
    //            new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc),
    //            new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc),
    //            true, 10, 2, 5);
    //        Event newEvent = new Event("Title", "Description",
    //            new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc),
    //            new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc),
    //            true, 10, 2, 5);

    //        testList.Add(testEvent);
    //        testList.Add(testEvent2);
    //        testList.Add(testEvent3);
    //        testList.Add(testEvent4);

    //    }

    //    [TestMethod]
    //    public void CreateEventAsyncTest()
    //    {
    //        //Arrange
    //        EventRepository newEventRepo = new EventRepository();

    //        //Act
    //        var result = newEventRepo.CreateEventAsync(testEvent);

    //        //Assert
    //        Assert.AreEqual(result, testEvent);
    //    }

    //    [TestMethod]
    //    public void CreateEventAsyncCreatesEventId()
    //    {
    //        //Arrange
    //        EventRepository newEventRepositoy = new EventRepository();
    //        int expectedResult = 1;

    //        //Act
    //        Event actualResult = newEventRepositoy.CreateEventAsync(testEvent);

    //        //Assert
    //        Assert.AreEqual(expectedResult, actualResult.EventId);
    //    }


    //    [TestMethod]
    //    public void UpdateEventTest()
    //    {
    //        //Arrange - we need a created event and a new event, datawise.
    //            //use testEvent - try to update that object.

    //        Event eventUpdate = new Event("Title", "Description",
    //        new DateTime(2025, 5, 8, 13, 05, 0, DateTimeKind.Utc),
    //        new DateTime(2025, 5, 8, 15, 05, 0, DateTimeKind.Utc),
    //        true, 10, 2, 5);
    //        EventRepository newEventRepo = new EventRepository();
    //        int eventId = 1;

    //        //Act
    //        testEvent.EventId = 1;
    //        Event result = newEventRepo.UpdateEventAsync(eventId, eventUpdate);
    //        result.EventId = 1;

    //        //Assert
    //        Assert.AreEqual(result, testEvent);

    //    }

    //    [TestMethod]
    //    public void TestEventIsDeletedFromDatabase()
    //    {
    //        //Arrange
    //        EventRepository eventRepository = new EventRepository();
    //        testEvent.EventId = 1;

    //        //Act
    //        Event newEvent = eventRepository.DeleteEvent(1);

    //        //Assert
    //        Assert.IsNull(testList[0]);

    //    }

    //    [TestMethod]
    //    public void TestGetAll()
    //    {
    //        //Arrange
    //        EventRepository eventRepository = new EventRepository();

    //        //Act
    //        Event expectedResult = testList.GetAll();

    //        //Assert
    //        Assert.AreEqual(testList.Count, expectedResult.Count);

    //    }

    //    [TestMethod]
    //    public void TestGetById()
    //    {
    //        //Arrange
    //        testEvent.EventId = 1;
    //        //testList.Add(testEvent); <--- burde vi ikke behøver grundet Setup, right?

    //        //Act
    //        List<Event> expectedResult = testList.GetById(1);

    //        //Assert
    //        Assert.AreEqual(testList.Count, expectedResult.Count);
    //    }
    //}
}
