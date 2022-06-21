namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public class UserFactory : IUserFactory
    {
        public User CreateLogin(UserId id, UserName name, EmailAddress email, Password password)
        {
            return new User(id, name, email, password);
        }

        public User CreateNew(UserName name, EmailAddress email, Password password)
        {
            var id = new UserId(Guid.NewGuid());
            return new User(id, name, email, password);
        }

        public User CreateFromRepository(UserId id, UserName name, EmailAddress email, string hashPassword)
        {
            var defaultPassword = new Password(User.DefaultPassword);
            var user = new User(id, name, email, defaultPassword)
            {
                HashPassword = hashPassword
            };
            return user;
        }


    }
}
