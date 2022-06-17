namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public class UserFactory : IUserFactory
    {
        public User CreateFromRepository(UserId id, UserName name, EmailAddress email, string hashPassword)
        {
            var user = new User(id, name, email)
            {
                HashPassword = hashPassword
            };
            return user;
        }

        public User CreateNew(UserName name, EmailAddress email, Password password)
        {
            var id = new UserId(Guid.NewGuid());
            return new User(id, name, email, password);
        }
    }
}
