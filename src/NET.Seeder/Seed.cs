namespace NET.Seeder
{
    public class Seed
    {
        public string Query { get; init; }
        public List<Parameter> Parameters { get; init; }

        public Seed(string query, List<Parameter> parameters)
        {
            Query = query;
            Parameters = parameters;
        }
    }
}
