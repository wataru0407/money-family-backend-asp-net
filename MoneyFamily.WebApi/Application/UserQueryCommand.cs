namespace MoneyFamily.WebApi.Application
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
