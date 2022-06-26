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
    }
}
