using Microsoft.AspNetCore.Mvc;
using MoneyFamily.WebApi.Application.Service;
using MoneyFamily.WebApi.Controllers;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Presentation.Secutiry;

namespace MoneyFamily.WebApi.Presentation.Controller
{
    public class AuthenticationControllerActions : ControllerBase, IAuthenticationController
    {
        private readonly AuthenticationAppricationService authService;
        private readonly JwtSetting jwtSetting;

        public AuthenticationControllerActions(AuthenticationAppricationService authService, JwtSetting jwtSetting)
        {
            this.authService = authService;
            this.jwtSetting = jwtSetting;
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

            var token = JwtHelper.GenToken(authrizedUser.Id, jwtSetting);
            var response = new LoginResponse() { Token = token };
            return Ok(response);
        }

        Task<ActionResult<LoginResponse>> IAuthenticationController.PasswordResetAsync(LoginRequest body)
        {
            throw new NotImplementedException();
        }
    }
}
