namespace MoneyFamily.WebApi.Application.Accounts.Update
{
    public class AccountUpdateCommand
    {
        public Guid Id { get; }
        public string Name { get; }

        public AccountUpdateCommand(Guid id, string name = null)
        {
            Id = id;
            Name = name;
        }
    }
}
