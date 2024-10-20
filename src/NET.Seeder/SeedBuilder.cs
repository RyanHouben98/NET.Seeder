using System.Linq.Expressions;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace NET.Seeder
{
    public class SeedBuilder<TEntity> where TEntity : class
    {
        private string _tableName;
        private Dictionary<string, string> _columnMappings;
        private Dictionary<string, object> _values;

        public SeedBuilder()
        {
            _tableName = typeof(TEntity).Name;
            _columnMappings = new Dictionary<string, string>();
            _values = new Dictionary<string, object>();
        }

        public SeedBuilder<TEntity> WithTableName(string tableName)
        {
            _tableName = tableName;

            return this;
        }

        public SeedBuilder<TEntity> WithColumn<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, string columnName)
        {
            if (propertyExpression.Body is MemberExpression memberExpression)
            {
                // Get the property name from the expression
                string propertyName = memberExpression.Member.Name;

                // Map the property name to the given column name
                _columnMappings[propertyName] = columnName;
            }
            else
            {
                throw new ArgumentException("Invalid expression. Must be a member expression.");
            }

            return this;
        }

        public SeedBuilder<TEntity> WithEntity(TEntity entity)
        {
            // Use reflection to get all properties of the entity
            var entityProperties = typeof(TEntity).GetProperties();

            foreach (var property in entityProperties)
            {
                // Get the property name
                string propertyName = property.Name;

                // Get the corresponding value from the entity instance
                var propertyValue = property.GetValue(entity);

                // Check if there's a custom column mapping; if not, default to the property name
                string columnName = _columnMappings.ContainsKey(propertyName)
                    ? _columnMappings[propertyName] // Use custom column name
                    : propertyName; // Default to property name if no custom mapping exists

                // Add the column and its value to the _columns dictionary
                _values[columnName] = propertyValue;
            }

            return this;
        }

        public Seed GenerateSeed()
        {
            if (string.IsNullOrEmpty(_tableName))
                throw new InvalidOperationException("Table name must be specified.");

            // Create column names and parameter placeholders
            var columns = string.Join(", ", _values.Keys);
            var placeholders = string.Join(", ", _values.Keys.Select((_, i) => $"@p{i}"));

            // Generate the SQL query
            var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({placeholders});";

            // Generate the parameters
            var parameters = _values.Values.Select((v, i) => new Parameter($"@p{i}", v)).ToList();

            // Return the Seed with query and parameters
            return new Seed(sql, parameters);
        }
    }
}
