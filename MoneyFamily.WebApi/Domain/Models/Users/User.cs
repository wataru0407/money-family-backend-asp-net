using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

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
            if (id is null) throw new ArgumentNullException(nameof(id));
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (email is null) throw new ArgumentNullException(nameof(email));
            if (password is null) throw new ArgumentNullException(nameof(password));

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

        public static User CreateInstance(
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

        //public string HashPassword()
        //{
        //    var solt = Encoding.UTF8.GetBytes(Name.Value);
        //    var hash = KeyDerivation.Pbkdf2(
        //        Password.Value,
        //        solt,
        //        prf: KeyDerivationPrf.HMACSHA256,
        //        iterationCount: 10000,
        //        numBytesRequested: 256 / 8
        //        );
        //    return Convert.ToBase64String(hash);
        //}

    }
}
