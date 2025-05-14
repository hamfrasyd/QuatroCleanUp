
using System.Transactions;
using Microsoft.Extensions.Configuration;
using QuatroCleanUpBackend;
/*
  IMPORTANT SETUP NOTE:
  To make sure this test can read your appsettings.json file at runtime,
  you must include the following in your test project's .csproj file:

  <ItemGroup>
    <None Include="appsettings.json">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
  </ItemGroup>

  Why this matters:
  1. We tell the build system “appsettings.json" is important data for our tests and it isn't code to compile.
  2. We ask it to place that file into the same folder where the test assembly ends up (usually bin/Debug/netX).
  3. That way, when the test runs, it can simply look next to itself for appsettings.json 
     and load the connection string (so no guessing about the file paths).
*/

namespace QuatroCleanUpEventTests
{
    /// <summary>
    /// This class contains integration tests for the EventRepository class.
    /// It uses the real database connection string from appsettings.json
    /// and then undoes any changes so your database stays clean. (See TransactionScope)
    /// </summary>
    /// <remarks>
    /// - We load appsettings.json so we get the same connection string your app uses.
    /// - We wrap each test in a transaction so that inserts are rolled back.
    /// - This lets us verify behavior without leaving test data behind in the database.
    /// </remarks>
    public class EventRepositoryIntergrationTests
    {
        private readonly EventRepository _repo;

        /// <summary>
        /// Constructor for the test class.
        /// Builds a configuration object that reads from appsettings.json,
        /// then uses that to create the EventRepository we will test.
        /// </summary>
        public EventRepositoryIntergrationTests()
        {
            // 1. Create a configuration builder object. This line is like a blank canvas.
            var builder = new ConfigurationBuilder();

            // 2. Tell it to load the file named "appsettings.json".
            //    - 'optional: false' means, fail immediately if the file is missing.
            //    - 'reloadOnChange: false' means, don't watch the file for live changes.
            builder.AddJsonFile(
                "appsettings.json",    // The name of the file containing our settings
                optional: false,       // Throw an error if this file is missing
                reloadOnChange: false  // We don’t need live‐reload in tests because the file never changes while running
            );

            // 3. Build the configuration object. Now it holds all the settings from the JSON.
            IConfiguration config = builder.Build();

            // 4. Pass that configuration into the real EventRepository constructor.
            //    Under the hood, the repository reads "QuatroCleanUpDb"
            //    connection string and stores it for later. (This variable is in appsettings.json)
            _repo = new EventRepository(config);
        }

        /// <summary>
        /// This test verifies that CreateEventAsync actually inserts a row into the database
        /// and returns an Event object whose EventId property was set to the newly created ID.
        /// </summary>
        /// <returns>A Task, because the test is asynchronous and a regular "async Task" is the same as "Void".</returns>
        [Fact]
        public async Task CreateEventAsync_InsertsRowAndReturnsNewId()
        {
            /* -------------------- 1. ARRANGE -------------------- */
            // Create a brand-new Event object with dummy values.
            var newEvent = new Event
            {
                Title = "Test Event ",
                Description = "Integration Test Insert",
                StartTime = DateTime.UtcNow,              // Time: right now
                EndTime = DateTime.UtcNow.AddHours(2),    // Time: two hours later
                FamilyFriendly = true,
                Participants = 0,
                TrashCollected = 0,
                StatusId = 1,  // This ID must already exist in the test database, since its a Foreign Key
                LocationId = 1 // This ID must already exist in the test database, since its a Foreign Key
            };

            /* -------------------- 2. START TRANSACTION -------------------- */
            // Here we open a TransactionScope, which is a special .NET class that lets us
            // group multiple database operations into a single “transaction.” Think of it like
            // putting all your changes into a box: either everything in the box gets saved,
            // or nothing does.
            // 
            // What is TransactionScope?
            //  - It’s part of System.Transactions.
            //  - It creates an “ambient” transaction that any ADO.NET calls automatically join.
            //  - If you don’t explicitly tell it “Complete,” then when the scope ends,
            //    everything inside gets rolled back (undone).
            //
            // What is TransactionScopeAsyncFlowOption.Enabled?
            //  - By default, TransactionScope doesn’t flow across async/await boundaries.
            //  - Passing AsyncFlowOption.Enabled tells it to “hang on to” the transaction
            //    even when we await asynchronous calls.
            //  - This ensures that our INSERT, which happens inside an async method,
            //    stays inside the same transaction until the using‐block finishes.
            //
            // In practical terms:
            //  - We start the transaction here.
            //  - We run our test’s CreateEventAsync (which opens connections, inserts, etc.).
            //  - When we exit this block without calling transaction.Complete(),
            //    the transaction is automatically rolled back.
            //  - That means our test leaves absolutely no data behind.
            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                /* -------------------- 3. ACT -------------------- */
                // Call the real CreateEventAsync method on our repository.
                // This will open a SQL connection, run the INSERT statement,
                // and the DB Primary Key IDENTITY will set newEvent.EventId to the generated ID.
                //That means:
                //  1. Open a SQL connection using the connection string from appsettings.json that we
                //    configured up in the constructor of this test class.
                //  2. Run the INSERT statement
                //  3. Retrieve the new EventId via SCOPE_IDENTITY()
                //       -  The DB Primary Key IDENTITY will set newEvent.EventId to the newEvent.Id.
                Event result = await _repo.CreateEventAsync(newEvent);

                /* -------------------- 4. ASSERT -------------------- */
                // Check that we returned a valid Event object (Not-Null).
                Assert.NotNull(result);

                // EventId is a positive integer (SQL IDENTITY starts at 1 and goes up).
                Assert.True(result.EventId > 0, $"Expected a positive EventId, but got {result.EventId}");

                // Note: We do NOT call transaction.Complete() here.
                //     - (The method that actually saves the newEvent in the DB, a little like ExecuteNonQuery)
                // Exiting this using-block without Complete() causes an automatic rollback,
                // so the INSERT is undone and the database stays clean.
            }

            /* -------------------- 5. END TRANSACTION -------------------- */
            // Once we exit the using-block, the TransactionScope is disposed,
            // which causes an automatic rollback of any database commands executed inside it.
        }
    }
}
