namespace MoneyFamily.WebApi.Application
{
    public class UserGetResult
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }

        public UserGetResult(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
