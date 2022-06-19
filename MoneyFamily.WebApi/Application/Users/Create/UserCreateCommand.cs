namespace MoneyFamily.WebApi.Application.Users.Create
{
    public class UserCreateCommand
    {
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }

        public UserCreateCommand(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
