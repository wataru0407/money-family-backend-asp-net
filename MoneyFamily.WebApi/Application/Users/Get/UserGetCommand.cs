namespace MoneyFamily.WebApi.Application.Users.Get
{
    public class UserGetCommand
    {
        public Guid Id { get; set; }

        public UserGetCommand(Guid id)
        {
            Id = id;
        }
    }
}
