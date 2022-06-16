namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public class HashUser
    {
        public UserId Id { get; }
        public UserName Name { get; private set; }
        public EmailAddress Email { get; private set; }
        public HashPassword Password { get; private set; }

        public HashUser(
            UserId id,
            UserName name,
            EmailAddress email,
            HashPassword password)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (email is null) throw new ArgumentNullException(nameof(email));
            if (password is null) throw new ArgumentNullException(nameof(password));

            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
