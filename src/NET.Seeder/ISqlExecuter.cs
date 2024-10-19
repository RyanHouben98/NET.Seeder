namespace NET.Seeder
{
    public interface ISqlExecutor
    {
        Task<int> ExecuteAsync(string sql, IEnumerable<Parameter> parameters);
    }
}
