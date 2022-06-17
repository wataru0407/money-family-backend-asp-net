namespace MoneyFamily.WebApi.Application
{
    public class UserUpdateResult
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }

        public UserUpdateResult(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
