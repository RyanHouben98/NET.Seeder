using System.Reflection;

namespace NET.Seeder
{
    public class EntityMapper
    {
        public static Dictionary<string, object> MapEntity<TEntity>(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            // Get all public instance properties
            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Create a dictionary of column names and their corresponding values
            var columnValues = properties
                .Where(p => p.CanRead)  // Only properties that can be read
                .ToDictionary(p => p.Name, p => p.GetValue(entity));

            return columnValues;
        }
    }
}
