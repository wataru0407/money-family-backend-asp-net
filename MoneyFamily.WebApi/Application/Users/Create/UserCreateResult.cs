namespace MoneyFamily.WebApi.Application.Users.Create
{
    public class UserCreateResult
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }

        public UserCreateResult(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
