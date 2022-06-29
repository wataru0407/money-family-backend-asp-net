namespace MoneyFamily.WebApi.Application.Accounts.RemoveMember
{
    public class AccountRemoveMemberCommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        public AccountRemoveMemberCommand(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
