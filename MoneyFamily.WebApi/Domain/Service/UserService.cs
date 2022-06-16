using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using System.Text;

namespace MoneyFamily.WebApi.Domain.Service
{
    public class UserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Exists(User user)
        {
            var found = await userRepository.FindByEmail(user.Email);
            return found != null;
        }

        public HashUser CreateHashUser(User user)
        {
            return new HashUser(
                user.Id,
                user.Name,
                user.Email,
                CreateHashPassword(user.Email, user.Password)
                );
        }
        public HashPassword CreateHashPassword(EmailAddress email, Password password)
        {
            var solt = Encoding.UTF8.GetBytes(email.Value);
            var hash = KeyDerivation.Pbkdf2(
                password.Value,
                solt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                );
            return new HashPassword(Convert.ToBase64String(hash));
        }
    }
}
