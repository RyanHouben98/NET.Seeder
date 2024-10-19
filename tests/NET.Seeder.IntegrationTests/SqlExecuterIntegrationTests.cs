using Npgsql;
using Testcontainers.PostgreSql;

namespace NET.Seeder.IntegrationTests
{
    public class SqlExecutorIntegrationTestWithTestContainers : IAsyncLifetime
    {
        private PostgreSqlContainer _postgreSqlContainer;
        private string _connectionString;

        public async Task InitializeAsync()
        {
            // Arrange: Spin up a PostgreSQL container using TestContainers
            _postgreSqlContainer = new PostgreSqlBuilder()
                .Build();

            // Start the container
            await _postgreSqlContainer.StartAsync();

            // Build connection string for the container
            _connectionString = _postgreSqlContainer.GetConnectionString();
        }

        public async Task DisposeAsync()
        {
            // Clean up: Stop and dispose the PostgreSQL container
            await _postgreSqlContainer.StopAsync();
        }

        [Fact]
        public async Task SqlExecutor_ShouldInsertDataIntoPostgreSqlDatabase()
        {
            // Arrange: Create a PostgreSQL connection
            using var connection = new NpgsqlConnection(_connectionString);

            // Create a table for testing
            string createTableSql = @"
                CREATE TABLE TestUser (
                    Id SERIAL PRIMARY KEY,
                    Username VARCHAR(100),
                    PasswordHash VARCHAR(100),
                    Email VARCHAR(100)
                );
            ";
            await connection.OpenAsync();
            using var createTableCommand = new NpgsqlCommand(createTableSql, connection);
            await createTableCommand.ExecuteNonQueryAsync();

            // Create the SqlExecutor
            var sqlExecutor = new SqlExecutor(connection);

            // Prepare SQL and parameters for inserting a test user
            string insertSql = "INSERT INTO TestUser (Username, PasswordHash, Email) VALUES (@p0, @p1, @p2);";
            var parameters = new List<Parameter>
            {
                new Parameter("@p0", "test_user"),
                new Parameter("@p1", "hashed_password"),
                new Parameter("@p2", "test@example.com")
            };

            // Act: Insert the data using SqlExecutor
            int rowsAffected = await sqlExecutor.ExecuteAsync(insertSql, parameters);

            // Assert: Check that one row was affected
            Assert.Equal(1, rowsAffected);

            // Query the data to verify insertion
            string selectSql = "SELECT Username, PasswordHash, Email FROM TestUser WHERE Username = 'test_user';";

            // Create a new connection for the SELECT command
            await using var selectConnection = new NpgsqlConnection(_connectionString);
            await selectConnection.OpenAsync();

            using var selectCommand = new NpgsqlCommand(selectSql, selectConnection);
            using var reader = await selectCommand.ExecuteReaderAsync();

            // Assert: Check that the inserted data matches
            Assert.True(await reader.ReadAsync());
            Assert.Equal("test_user", reader.GetString(0));
            Assert.Equal("hashed_password", reader.GetString(1));
            Assert.Equal("test@example.com", reader.GetString(2));

            // Clean up: Delete the test user
            string deleteSql = "DELETE FROM TestUser WHERE Username = 'test_user';";
            using var deleteCommand = new NpgsqlCommand(deleteSql, connection);
            await deleteCommand.ExecuteNonQueryAsync();
        }

    }
}
