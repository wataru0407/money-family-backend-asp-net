namespace MoneyFamily.WebApi.Application
{
    public class UserGetCommand
    {
        public Guid Id { get; }

        public UserGetCommand(Guid id)
        {
            Id = id;
        }
    }
}
