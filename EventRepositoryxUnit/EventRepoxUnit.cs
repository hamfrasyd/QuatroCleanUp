using System.Transactions;
using Microsoft.Extensions.Configuration;
using QuatroCleanUpBackend;

namespace EventRepositoryxUnit
{
    public class EventRepoxUnit
    {
        private readonly EventRepository _repo;
        private readonly string _connectionString;

        public EventRepoxUnit()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: false
            );
            IConfiguration config = builder.Build();
            _repo = new EventRepository(config);
        }

        [Fact]
        public async Task CreateEventTransactionTest()
        {
            /* -------------------- 1. ARRANGE -------------------- */
            var newEvent = new Event
            {
                Title = "Test Event ",
                Description = "Integration Test Insert",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(2),
                FamilyFriendly = true,
                Participants = 0,
                TrashCollected = 0,
                StatusId = 1,
                LocationId = 1
            };

            /* -------------------- 2. START TRANSACTION -------------------- */
            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                /* -------------------- 3. ACT -------------------- */
                Event result = await _repo.CreateEventAsync(newEvent);

                /* -------------------- 4. ASSERT -------------------- */
                Assert.NotNull(result);
                Assert.True(result.EventId > 0, $"Expected a positive EventId, but got {result.EventId}");
            }
            /* -------------------- 5. END TRANSACTION -------------------- */


        }

        //public async Task GetAllAsyncTransactionTest()
        //{
        //    using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //    {
        //        EventRepository newRepository = new EventRepository();
        //        await newRepository.CreateEventAsync(new Event { Title = "Event 1", StatusId = 1, LocationId = 1 });
        //        await newRepository.CreateEventAsync(new Event { Title = "Event 2", StatusId = 1, LocationId = 1 });
        //
        //        var events = await newRepository.GetAllAsync();
        //
        //        Assert.NotNull(events);
        //        Assert.Equal(2, events.Count());
        //        Assert.Contains(events, e => e.Title == "Event 1");
        //        Assert.Contains(events, e => e.Title == "Event 2");
        //    }
        //}

        //public async Task GetByIdAsyncTransactionTest()
        //{
        //    using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //    {
        //        EventRepository newRepository = new EventRepository();
        //    }
        //}


    }
}