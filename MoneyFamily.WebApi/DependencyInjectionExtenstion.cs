using MoneyFamily.WebApi.Application.Accounts;
using MoneyFamily.WebApi.Application.Users;
using MoneyFamily.WebApi.Controllers;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using MoneyFamily.WebApi.Domain.Service;
using MoneyFamily.WebApi.Infrastructure.Repository;
using MoneyFamily.WebApi.Presentation.Controller;
using MoneyFamily.WebApi.Presentation.Controllers;

namespace MoneyFamily.WebApi
{
    public static class DependencyInjectionExtenstion
    {
        public static WebApplicationBuilder AddDependencyInjection(WebApplicationBuilder builder)
        {
            builder.Services
                // Controller
                .AddTransient<IAuthenticationController, AuthController>()
                .AddTransient<IUsersController, UserController>()
                .AddTransient<IAccountsController, AccountController>()
                // ApplicationService
                .AddTransient<UserApplicationService>()
                .AddTransient<AccountApplicationService>()
                // DomainService
                .AddTransient<UserService>()
                // Repository
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IAccountRepository, AccountRepository>()
                // Factory
                .AddTransient<IUserFactory, UserFactory>()
                ;
            return builder;
        }
    }
}
