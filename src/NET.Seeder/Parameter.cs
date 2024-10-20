namespace NET.Seeder
{
    public class Parameter
    {
        public string Name { get; init; }
        public object Value { get; init; }

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
