using NET.Seeder.UnitTests.Entities;

namespace NET.Seeder.UnitTests
{
    public class SeederUnitTests
    {
        [Fact]
        public void Add_WithEntity_ShouldAddEntityToEntitiesList()
        {
            // Arrange
            var seeder = new Seeder();

            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "johndoe@email.com"
            };

            // Act
            seeder.Add(user);

            // Assert
            Assert.Single(seeder.Entities);
        }

        [Fact]
        public void AddRange_WithEntities_ShouldAddEntitiesToEntitiesList()
        {
            // Arrange
            var seeder = new Seeder();

            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "johndoe@email.com"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    EmailAddress = "janedoe@email.com"
                }
            };

            // Act 
            seeder.AddRange(users);

            // Assert
            Assert.Equal(users, seeder.Entities);
        }
    }
}
