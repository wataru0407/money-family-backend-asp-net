using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public class User
    {
        public const string DefaultPassword = "af7785cc1fde29d13927843c50be3213";
        private const string DefaultHashPassword = "e88a50133076de7279c15fbfd4ec1157";

        public UserId Id { get; }
        public UserName Name { get; private set; }
        public EmailAddress Email { get; private set; }
        public Password Password { get; private set; }

        // パスワードをハッシュ化したものを取得したい場合HashPasswordを使用する
        private string _hashPassword = DefaultHashPassword;
        public string HashPassword
        {
            get
            {
                if (_hashPassword == DefaultHashPassword)
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

        private string ConvertHashPassword()
        {
            var solt = Encoding.UTF8.GetBytes(Id.Value.ToString());
            var hash = KeyDerivation.Pbkdf2(
                Password.Value,
                solt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
            return Convert.ToBase64String(hash);
        }

    }
}
