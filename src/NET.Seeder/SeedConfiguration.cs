namespace NET.Seeder
{
    public abstract class SeedConfiguration<TEntity> : ISeedConfiguration<TEntity> where TEntity : class
    {
        public abstract void Configure(SeedBuilder<TEntity> seedBuilder);
    }
}
