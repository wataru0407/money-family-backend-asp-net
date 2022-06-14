namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public class User
    {
        public UserId Id { get; }
        public UserName Name { get; private set; }
        public EmailAddress Email { get; private set; }
        public Password Password { get; private set; }

        private User(
            UserId id,
            UserName name,
            EmailAddress email,
            Password password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }

        public static User CreateNew(
            UserName userName,
            EmailAddress email,
            Password password)
        {
            return new User(
                new UserId(Guid.NewGuid()),
                userName,
                email,
                password);
        }

        public static User CreateFromRepository(
            UserId userId,
            UserName name,
            EmailAddress email,
            Password password)
        {
            return new User(
                userId,
                name,
                email,
                password);
        }

        public void ChangeName(UserName name)
        {
            Name = name;
        }

        public void ChageEmail(EmailAddress email)
        {
            Email = email;
        }

        public void ChangePassword(Password password)
        {
            Password = password;
        }


    }
}
