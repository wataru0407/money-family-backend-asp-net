namespace MoneyFamily.WebApi.Application.Users.Query
{
    public class UserQueryCommand
    {
        public string Email { get; }

        public UserQueryCommand(string email)
        {
            Email = email;
        }
    }
}
