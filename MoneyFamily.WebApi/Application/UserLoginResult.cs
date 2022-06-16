namespace MoneyFamily.WebApi.Application
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
