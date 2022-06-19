namespace MoneyFamily.WebApi.Application.Users.Login
{
    public class UserLoginResult
    {
        public Guid Id { get; }

        public UserLoginResult(Guid id)
        {
            Id = id;
        }
    }
}
