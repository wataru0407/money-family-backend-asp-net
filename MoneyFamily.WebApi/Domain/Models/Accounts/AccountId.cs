namespace MoneyFamily.WebApi.Domain.Models.Accounts
{
    public record AccountId
    {
        public Guid Value { get; }

        public AccountId(Guid value)
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
