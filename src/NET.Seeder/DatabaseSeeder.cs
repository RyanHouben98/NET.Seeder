namespace NET.Seeder
{
    public class DatabaseSeeder
    {
        private readonly ISqlExecutor _executor;

        public DatabaseSeeder(ISqlExecutor executor)
        {
            _executor = executor;
        }

        public async Task SeedAsync<T>(SeedBuilder<T> seedBuilder) where T : class
        {
            // Generate SQL and parameters
            var sql = seedBuilder.GenerateSql(out var parameters);

            // Execute the generated SQL with parameters
            await _executor.ExecuteAsync(sql, parameters);
        }
    }
}
