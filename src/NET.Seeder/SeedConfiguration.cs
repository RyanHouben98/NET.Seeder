namespace NET.Seeder
{
    public abstract class SeedConfiguration<TEntity> where TEntity : class
    {
        public abstract void Configure(SeedBuilder<TEntity> builder);
    }
}
