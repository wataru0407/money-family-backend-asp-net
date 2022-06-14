using Microsoft.AspNetCore.Mvc;
using MoneyFamily.WebApi.Application.Service;
using MoneyFamily.WebApi.AuthTest.JwtHelpers;
using MoneyFamily.WebApi.AuthTest.Models;
using MoneyFamily.WebApi.Controllers;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Presentation.Secutiry;

namespace MoneyFamily.WebApi.Presentation.Controller
{
    public class AuthenticationControllerActions : ControllerBase, IAuthenticationController
    {
        private readonly AuthenticationAppricationService authService;
        private readonly JwtSettings jwtSettings;

        private IEnumerable<Users> logins = new List<Users>() {
            new Users() {
                    Id = Guid.NewGuid(),
                        EmailId = "admin@gmail.com",
                        UserName = "admin",
                        Password = "admin",
                },
                new Users() {
                    Id = Guid.NewGuid(),
                        EmailId = "test@gmail.com",
                        UserName = "test",
                        Password = "test",
                }
        };

        public AuthenticationControllerActions(AuthenticationAppricationService authService, JwtSettings jwtSettings)
        {
            this.authService = authService;
            this.jwtSettings = jwtSettings;
        }

        Task<ActionResult<UserResponse>> IAuthenticationController.CreateUserAsync(UserRequest body)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest body)
        {
            var email = new EmailAddress(body.Email);
            var password = new Password(body.Password);
            var authrizedUser = await authService.Login(email, password);
            if (authrizedUser == null)
            {
                return BadRequest($"wrong password");
            }

            var token = JwtHelper.GenToken(authrizedUser.Id, jwtSettings);
            var response = new LoginResponse() { Token = token};
            return Ok(response);

            //try
            //{
            //    //var Token = new UserTokens();
            //    var response = new LoginResponse();
            //    var Valid = logins.Any(x => x.EmailId.Equals(body.Email, StringComparison.OrdinalIgnoreCase));
            //    if (Valid)
            //    {
            //        var user = logins.FirstOrDefault(x => x.EmailId.Equals(body.Email, StringComparison.OrdinalIgnoreCase));
            //        var token = JwtHelpers.GenTokenkey(jwtSettings).Token;
            //        response.Token = token;

            //    }
            //    else
            //    {
            //        return BadRequest($"wrong password");
            //    }
            //    return response;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            //throw new NotImplementedException();
        }

        Task<ActionResult<LoginResponse>> IAuthenticationController.PasswordResetAsync(LoginRequest body)
        {
            throw new NotImplementedException();
        }
    }
}
