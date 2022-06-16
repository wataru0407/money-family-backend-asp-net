namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public record HashPassword
    {
        public string Value { get; }

        public HashPassword(string value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Value = value;
        }
    }

}
