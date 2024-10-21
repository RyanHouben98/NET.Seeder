using Npgsql;

namespace NET.Seeder
{
    public class Seeder
    {
        public List<object> Entities;
        private string _connectionString;

        public Seeder()
        {
            Entities = new List<object>();
        }

        public void AddPostgreConnecrtion(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(object entity)
        {
            Entities.Add(entity);
        }

        public void AddRange(IEnumerable<object> entities)
        {
            Entities.AddRange(entities);
        }

        public List<Seed> GenerateSeeds()
        {
            var seeds = new List<Seed>();

            foreach (var entity in Entities)
            {
                var entityType = entity.GetType();

                // Automatically find the configuration for the entity type
                var configurationType = typeof(ISeedConfiguration<>).MakeGenericType(entityType);

                // Create an instance of the configuration type
                var configurationInstance = FindConfigurationInstance(configurationType);

                if (configurationInstance == null)
                {
                    throw new InvalidOperationException($"No configuration found for entity type {entityType.Name}.");
                }

                var builderType = typeof(SeedBuilder<>).MakeGenericType(entityType);
                var builderInstance = Activator.CreateInstance(builderType);

                // Invoke the Configure method on the configuration instance
                var configureMethod = configurationType.GetMethod("Configure");
                configureMethod?.Invoke(configurationInstance, new[] { builderInstance });

                // Set the entity in the builder
                var withEntityMethod = builderType.GetMethod("WithEntity");
                withEntityMethod?.Invoke(builderInstance, new[] { entity });

                // Generate and add the seed
                var generateSeedMethod = builderType.GetMethod("GenerateSeed");
                var seed = generateSeedMethod?.Invoke(builderInstance, null) as Seed;

                if (seed != null)
                {
                    seeds.Add(seed);
                }
            }

            return seeds;
        }

        public async Task SeedAsync(List<Seed> seeds)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        foreach (var seed in seeds)
                        {
                            using (var command = new NpgsqlCommand(seed.Query, connection, transaction))
                            {
                                foreach(var parameter in seed.Parameters)
                                {
                                    command.Parameters.Add(new NpgsqlParameter(parameter.Name, parameter.Value));
                                }

                                await command.ExecuteNonQueryAsync();
                            }
                        }

                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();

                        throw;
                    }
                }
            }
        }

        private object FindConfigurationInstance(Type configurationType)
        {
            // Search for a concrete class that implements the generic interface
            var configType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(type => configurationType.IsAssignableFrom(type) && !type.IsAbstract);

            return configType != null ? Activator.CreateInstance(configType) : null;
        }
    }
}
