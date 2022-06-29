namespace MoneyFamily.WebApi.Application.Accounts.GetAll
{
    public class AccountGetAllResult
    {
        public Guid Id { get; }
        public string Name { get; }

        public Guid CreateUserId { get; }

        public List<Guid> Members { get; }

        public AccountGetAllResult(Guid id, string name, Guid createUserId, List<Guid> members)
        {
            Id = id;
            Name = name;
            CreateUserId = createUserId;
            Members = members;
        }

    }
}
