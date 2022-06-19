namespace MoneyFamily.WebApi.Application.Users.Update
{
    public class UserUpdateCommand
    {
        public Guid Id { get; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public UserUpdateCommand(Guid id)
        {
            Id = id;
        }
    }
}
