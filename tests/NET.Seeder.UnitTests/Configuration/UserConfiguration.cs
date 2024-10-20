using NET.Seeder.UnitTests.Entities;

namespace NET.Seeder.UnitTests.Configuration
{
    public class UserConfiguration : ISeedConfiguration<User>
    {
        public void Configure(SeedBuilder<User> builder)
        {
            builder.WithTableName("users");

            builder.WithColumn(u => u.Id, "Id");
            builder.WithColumn(u => u.FirstName, "first_name");
            builder.WithColumn(u => u.LastName, "last_name");
            builder.WithColumn(u => u.EmailAddress, "EmailAddress");
        }
    }
}
