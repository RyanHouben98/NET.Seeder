namespace NET.Seeder
{
    public class SeedBuilder<T> where T : class
    {
        private string _tableName;
        private Dictionary<string, object> _columns;

        public SeedBuilder()
        {
            // Default values based on the entity type
            var (tableName, columnValues) = EntityMapper.MapEntity(Activator.CreateInstance<T>());
            _tableName = tableName;
            _columns = columnValues;
        }

        public SeedBuilder<T> WithTableName(string tableName)
        {
            _tableName = tableName;
            return this;
        }

        public SeedBuilder<T> WithEntity(T entity)
        {
            // Map the entity to columns and values using EntityMapper
            var (_, columnValues) = EntityMapper.MapEntity(entity);
            _columns = columnValues;
            return this;
        }

        public string GenerateSql(out List<Parameter> parameters)
        {
            if (string.IsNullOrEmpty(_tableName)) throw new InvalidOperationException("Table name must be specified.");

            var columns = string.Join(", ", _columns.Keys);
            var placeholders = string.Join(", ", _columns.Keys.Select((_, i) => $"@p{i}"));

            var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({placeholders});";
            parameters = _columns.Values.Select((v, i) => new Parameter($"@p{i}", v)).ToList();

            return sql;
        }
    }
}
