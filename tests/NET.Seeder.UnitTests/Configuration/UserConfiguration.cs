namespace NET.Seeder.UnitTests.Configuration
{
    public class UserConfiguration : SeedConfiguration<User>
    {
        public override void Configure(SeedBuilder<User> builder)
        {
            builder.WithTableName("users");

            builder.WithColumn(u => u.Id, "Id");
            builder.WithColumn(u => u.FirstName, "first_name");
            builder.WithColumn(u => u.LastName, "last_name");
            builder.WithColumn(u => u.EmailAddress, "EmailAddress");
        }
    }
}
