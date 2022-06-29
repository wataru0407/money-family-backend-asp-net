namespace MoneyFamily.WebApi.Application.Accounts.Get
{
    public class AccountGetCommand
    {
        public Guid Id { get; }

        public AccountGetCommand(Guid id)
        {
            Id = id;
        }
    }
}
