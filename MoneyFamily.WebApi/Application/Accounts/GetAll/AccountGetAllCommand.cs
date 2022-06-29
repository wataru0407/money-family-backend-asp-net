namespace MoneyFamily.WebApi.Application.Accounts.GetAll
{
    public class AccountGetAllCommand
    {
        public Guid UserId { get; }

        public AccountGetAllCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
