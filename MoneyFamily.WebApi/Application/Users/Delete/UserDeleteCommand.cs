namespace MoneyFamily.WebApi.Application.Users.Delete
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
