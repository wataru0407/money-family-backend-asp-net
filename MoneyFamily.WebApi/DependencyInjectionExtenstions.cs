using MoneyFamily.WebApi.Application.Service;
using MoneyFamily.WebApi.Controllers;
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
                .AddTransient<IAuthenticationController, AuthenticationControllerActions>()
                .AddTransient<UserAppricationService>()
                .AddTransient<UserService>()
                .AddTransient<IUserRepository, UserRepository>()
                ;
            return builder;
        }
    }
}
