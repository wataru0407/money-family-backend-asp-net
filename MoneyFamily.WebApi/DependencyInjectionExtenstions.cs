using MoneyFamily.WebApi.Application.Users;
using MoneyFamily.WebApi.Controllers;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using MoneyFamily.WebApi.Domain.Service;
using MoneyFamily.WebApi.Infrastructure.Repository;
using MoneyFamily.WebApi.Presentation.Controller;

namespace MoneyFamily.WebApi
{
    public static class DependencyInjectionExtenstions
    {
        public static WebApplicationBuilder AddDependencyInjection(WebApplicationBuilder builder)
        {
            builder.Services
                .AddTransient<IAuthenticationController, AuthController>()
                .AddTransient<IUsersController, UserController>()
                .AddTransient<UserAppricationService>()
                .AddTransient<UserService>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IUserFactory, UserFactory>()
                ;
            return builder;
        }
    }
}
