namespace MoneyFamily.WebApi.Application
{
    public class UserQueryResult
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }

        public UserQueryResult(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
