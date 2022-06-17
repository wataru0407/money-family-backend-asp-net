namespace MoneyFamily.WebApi.Application
{
    public class UserUpdateCommand
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }

        public UserUpdateCommand(Guid id, string name = null, string email = null, string password = null)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
