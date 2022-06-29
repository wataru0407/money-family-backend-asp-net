namespace MoneyFamily.WebApi.Application.Accounts.Delete
{
    public class AccountDeleteCommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        public AccountDeleteCommand(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
