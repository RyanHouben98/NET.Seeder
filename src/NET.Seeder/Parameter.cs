namespace NET.Seeder
{
    /// <summary>
    /// Represents a parameter to be used in a SQL query.
    /// </summary>
    public class Parameter
    {
        public string Name { get; }
        public object Value { get; }

        public Parameter(string name, object value)
        {
            Name = name;
            Value = value ?? DBNull.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Parameter other)
            {
                return Name == other.Name && Equals(Value, other.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value);
        }
    }
}
