namespace NET.Seeder.UnitTests
{
    public class SeedBuilderUnitTests
    {
        [Fact]
        public void GenerateSql_ShouldGenerateCorrectInsertStatement_ForUserEntity()
        {
            // Arrange: Create a User entity
            var user = new User
            {
                Id = 1,
                Username = "john_doe",
                PasswordHash = "secureHash",
                Email = "john@example.com"
            };

            // Act: Build the seed for the User entity
            var seedBuilder = new SeedBuilder<User>().WithEntity(user);
            var sql = seedBuilder.GenerateSql(out var parameters);

            // Assert: Check that the SQL is generated correctly
            var expectedSql = "INSERT INTO User (Id, Username, PasswordHash, Email) VALUES (@p0, @p1, @p2, @p3);";
            Assert.Equal(expectedSql, sql);

            // Assert: Check that the parameters are mapped correctly
            Assert.Collection(parameters,
                p0 => Assert.Equal(new Parameter("@p0", 1), p0),
                p1 => Assert.Equal(new Parameter("@p1", "john_doe"), p1),
                p2 => Assert.Equal(new Parameter("@p2", "secureHash"), p2),
                p3 => Assert.Equal(new Parameter("@p3", "john@example.com"), p3)
            );
        }
    }
}
