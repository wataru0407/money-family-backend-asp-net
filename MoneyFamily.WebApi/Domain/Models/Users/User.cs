using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public class User
    {

        public UserId Id { get; }
        public UserName Name { get; private set; }
        public EmailAddress Email { get; private set; }
        public Password? Password { get; private set; } = null;

        private string? _hashPassword;
        public string HashPassword
        {
            get
            {
                if (_hashPassword == null)
                {
                    return ConvertHashPassword();
                }
                return _hashPassword;
            }
            set
            {
                _hashPassword = value;
            }
        }

        public User(
            UserId id,
            UserName name,
            EmailAddress email,
            Password password)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public User(
            UserId id,
            UserName name,
            EmailAddress email
            )
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        //public static User CreateNew(
        //    UserName userName,
        //    EmailAddress email,
        //    Password password)
        //{
        //    return new User(
        //        new UserId(Guid.NewGuid()),
        //        userName,
        //        email,
        //        password);
        //}

        //public static User CreateFromRepository(
        //    UserId userId,
        //    UserName name,
        //    EmailAddress email,
        //    string hashPassword
        //    )
        //{
        //    //HashPassword = hashPassword;
        //    return new User(
        //        userId,
        //        name,
        //        email,
        //        Password.Empty
        //        );
        //}

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

        public string ConvertHashPassword()
        {
            var solt = Encoding.UTF8.GetBytes(Id.Value.ToString());
            var hash = KeyDerivation.Pbkdf2(
                Password.Value,
                solt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                );
            return Convert.ToBase64String(hash);
        }

    }
}
