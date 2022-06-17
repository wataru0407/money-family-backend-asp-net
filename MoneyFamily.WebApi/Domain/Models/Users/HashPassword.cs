namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public record HashPassword2
    {
        public string Value { get; }

        public HashPassword2(string value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Value = value;
        }
    }

}
