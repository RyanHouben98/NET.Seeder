using Npgsql;
using System.Data;

namespace NET.Seeder
{
    public class SqlExecutor : ISqlExecutor
    {
        private readonly NpgsqlConnection _connection;

        public SqlExecutor(NpgsqlConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task<int> ExecuteAsync(string sql, IEnumerable<Parameter> parameters)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            foreach (var param in parameters)
            {
                var dbParam = command.CreateParameter();
                dbParam.ParameterName = param.Name;
                dbParam.Value = param.Value ?? DBNull.Value;
                command.Parameters.Add(dbParam);
            }

            // Open the connection asynchronously if it's not open
            if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();  // Open the PostgreSQL connection asynchronously
            }

            // Execute the command
            return await command.ExecuteNonQueryAsync();
        }
    }
}
