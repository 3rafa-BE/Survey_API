namespace Survey.Abstractions
{
    public record Error(string code , string description , int? StatueCode)
    {
        public static Error None => new(string.Empty , string.Empty , null);
    }
}
