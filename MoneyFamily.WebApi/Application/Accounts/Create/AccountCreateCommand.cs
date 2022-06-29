namespace MoneyFamily.WebApi.Application.Accounts.Create
{
    public class AccountCreateCommand
    {
        public string Name { get; }
        public Guid CreateUserId { get; }

        public AccountCreateCommand(string name, Guid createUserId)
        {
            Name = name;
            CreateUserId = createUserId;
        }
    }
}
