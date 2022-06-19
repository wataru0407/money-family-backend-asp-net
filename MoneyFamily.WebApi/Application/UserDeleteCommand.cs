namespace MoneyFamily.WebApi.Application
{
    public class UserDeleteCommand
    {
        public Guid Id { get; }

        public UserDeleteCommand(Guid id)
        {
            Id = id;
        }
    }
}
