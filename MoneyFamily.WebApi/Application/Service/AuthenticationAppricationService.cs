using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using MoneyFamily.WebApi.Domain.Service;

namespace MoneyFamily.WebApi.Application.Service
{
    public class AuthenticationAppricationService
    {
        private readonly IUserRepository userRepository;
        private readonly UserService userService;

        public AuthenticationAppricationService(IUserRepository userRepository, UserService userService)
        {
            this.userRepository = userRepository;
            this.userService = userService;
        }

        public async Task<User> Login(EmailAddress email, Password password)
        {
            var found = await userRepository.FindByEmail(email);
            if (found?.Password == password)
            {
                return found;
            }
            return null;
        }
    }
}
