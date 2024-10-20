namespace NET.Seeder
{
    public interface ISeedConfiguration<T> where T : class
    {
        void Configure(SeedBuilder<T> seedBuilder);
    }
}
