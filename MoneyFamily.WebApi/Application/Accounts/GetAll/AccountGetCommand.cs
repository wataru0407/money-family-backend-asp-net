namespace MoneyFamily.WebApi.Application.Accounts.GetAll
{
    public class AccountGetCommand
    {
        public Guid UserId { get; }

        public AccountGetCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
