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
        public async Task AddRange_WithEntities_ShouldAddEntitiesToEntitiesList()
        {
            // Arrange
            var seeder = new Seeder();

            seeder.AddPostgreConnecrtion("Host=127.0.0.1;Port=5432;Database=seedertest;Username=postgres;Password=Welkom1!");

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

            var seeds = seeder.GenerateSeeds();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(users, seeder.Entities);
                Assert.Collection(seeds,
                    seed1 =>
                    {
                        Assert.Equal("INSERT INTO users (id, first_name, last_name, email_address) VALUES (@p0, @p1, @p2, @p3);", seed1.Query);
                        Assert.Collection(seed1.Parameters,
                            p0 => Assert.Equal(new Parameter("@p0", 1), p0),
                            p1 => Assert.Equal(new Parameter("@p1", "John"), p1),
                            p2 => Assert.Equal(new Parameter("@p2", "Doe"), p2),
                            p3 => Assert.Equal(new Parameter("@p3", "johndoe@email.com"), p3));

                    },
                    seed2 =>
                    {
                        Assert.Equal("INSERT INTO users (id, first_name, last_name, email_address) VALUES (@p0, @p1, @p2, @p3);", seed2.Query);
                        Assert.Collection(seed2.Parameters,
                            p0 => Assert.Equal(new Parameter("@p0", 2), p0),
                            p1 => Assert.Equal(new Parameter("@p1", "Jane"), p1),
                            p2 => Assert.Equal(new Parameter("@p2", "Doe"), p2),
                            p3 => Assert.Equal(new Parameter("@p3", "janedoe@email.com"), p3));

                    });
            });
        }
    }
}
