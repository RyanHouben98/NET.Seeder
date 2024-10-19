using System.Reflection;

namespace NET.Seeder
{
    public class EntityMapper
    {
        public static (string tableName, Dictionary<string, object> columnValues) MapEntity<T>(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            // Default table name is the entity class name
            string tableName = typeof(T).Name;

            // Get all public instance properties
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Create a dictionary of column names and their corresponding values
            var columnValues = properties
                .Where(p => p.CanRead)  // Only properties that can be read
                .ToDictionary(p => p.Name, p => p.GetValue(entity));

            return (tableName, columnValues);
        }
    }
}
