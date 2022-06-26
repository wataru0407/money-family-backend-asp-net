using MoneyFamily.WebApi.Application.Users;
using MoneyFamily.WebApi.Controllers;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using MoneyFamily.WebApi.Domain.Service;
using MoneyFamily.WebApi.Infrastructure.Repository;
using MoneyFamily.WebApi.Presentation.Controller;

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
                // ApplicationService
                .AddTransient<UserApplicationService>()
                // DomainService
                .AddTransient<UserService>()
                // Repository
                .AddTransient<IUserRepository, UserRepository>()
                // Factory
                .AddTransient<IUserFactory, UserFactory>()
                ;
            return builder;
        }
    }
}
