namespace MoneyFamily.WebApi.Application.Accounts.AddMember
{
    public class AccountAddMemberCommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        public AccountAddMemberCommand(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
