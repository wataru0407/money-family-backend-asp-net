namespace MoneyFamily.WebApi.Application
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
