namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public record UserId
    {
        public Guid Value { get; }

        public UserId(Guid value)
        {
            if (value.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(value));

            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
