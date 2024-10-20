using NET.Seeder.UnitTests.Entities;

namespace NET.Seeder.UnitTests
{
    public class SeedBuilderUnitTests
    {
        [Fact]
        public void GenerateSql_WithoutSpecifiedTableName_ShouldUseEntityNameAsTableName()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "johndoe@email.com"
            };

            var expectedSql = "INSERT INTO User (Id, FirstName, LastName, EmailAddress) VALUES (@p0, @p1, @p2, @p3);";

            // Act
            var seedBuilder = new SeedBuilder<User>()
                .WithEntity(user);

            var seed = seedBuilder.GenerateSeed();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(seed);
                Assert.Equal(expectedSql, seed.Query);
                Assert.Collection(seed.Parameters,
                    p0 => Assert.Equal(new Parameter("@p0", 1), p0),
                    p1 => Assert.Equal(new Parameter("@p1", "John"), p1),
                    p2 => Assert.Equal(new Parameter("@p2", "Doe"), p2),
                    p3 => Assert.Equal(new Parameter("@p3", "johndoe@email.com"), p3)
                );
            });
        }

        [Fact]
        public void GenerateSql_WithSpecifiedTableName_ShouldUseSpecifiedNameAsTableName()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "johndoe@email.com"
            };

            var expectedSql = "INSERT INTO users (Id, FirstName, LastName, EmailAddress) VALUES (@p0, @p1, @p2, @p3);";

            // Act
            var seedBuilder = new SeedBuilder<User>()
                .WithTableName("users")
                .WithEntity(user);

            var seed = seedBuilder.GenerateSeed();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(seed);
                Assert.Equal(expectedSql, seed.Query);
                Assert.Collection(seed.Parameters,
                    p0 => Assert.Equal(new Parameter("@p0", 1), p0),
                    p1 => Assert.Equal(new Parameter("@p1", "John"), p1),
                    p2 => Assert.Equal(new Parameter("@p2", "Doe"), p2),
                    p3 => Assert.Equal(new Parameter("@p3", "johndoe@email.com"), p3)
                );
            });
        }

        [Fact]
        public void GenerateSql_WithMappedColumns_ShouldUseSpecifiedNameAsTableName()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "johndoe@email.com"
            };

            var expectedSql = "INSERT INTO users (Id, first_name, last_name, email_address) VALUES (@p0, @p1, @p2, @p3);";

            // Act
            var seedBuilder = new SeedBuilder<User>()
                .WithTableName("users")
                .WithColumn(user => user.FirstName, "first_name")
                .WithColumn(user => user.LastName, "last_name")
                .WithColumn(user => user.EmailAddress, "email_address")
                .WithEntity(user);

            var seed = seedBuilder.GenerateSeed();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(seed);
                Assert.Equal(expectedSql, seed.Query);
                Assert.Collection(seed.Parameters,
                    p0 => Assert.Equal(new Parameter("@p0", 1), p0),
                    p1 => Assert.Equal(new Parameter("@p1", "John"), p1),
                    p2 => Assert.Equal(new Parameter("@p2", "Doe"), p2),
                    p3 => Assert.Equal(new Parameter("@p3", "johndoe@email.com"), p3)
                );
            });
        }
    }
}
