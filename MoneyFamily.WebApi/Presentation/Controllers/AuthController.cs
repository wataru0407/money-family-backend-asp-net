using Microsoft.AspNetCore.Mvc;
using MoneyFamily.WebApi.Application.Exceptions;
using MoneyFamily.WebApi.Application.Users;
using MoneyFamily.WebApi.Application.Users.Create;
using MoneyFamily.WebApi.Application.Users.Login;
using MoneyFamily.WebApi.Controllers;
using MoneyFamily.WebApi.Presentation.Secutiry;

namespace MoneyFamily.WebApi.Presentation.Controller
{
    public class AuthController : ControllerBase, IAuthenticationController
    {
        private readonly UserApplicationService userAppricationService;
        private readonly JwtSetting jwtSetting;

        public AuthController(UserApplicationService userAppricationService, JwtSetting jwtSetting)
        {
            this.userAppricationService = userAppricationService;
            this.jwtSetting = jwtSetting;
        }

        public async Task<ActionResult<UserResponse>> CreateUserAsync(UserRequest body)
        {
            try
            {
                var command = new UserCreateCommand(
                    body.Name,
                    body.Email,
                    body.Password
                    );
                var result = await userAppricationService.CreateUser(command);
                var response = new UserResponse()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Email = result.Email,
                };
                return Created(response.Id.ToString(), response);
            }
            catch (CustomDuplicateException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest body)
        {
            try
            {
                var command = new UserLoginCommand(body.Email, body.Password);
                var result = await userAppricationService.Login(command);
                var token = JwtHelper.GenToken(result.Id, jwtSetting);
                var response = new LoginResponse()
                {
                    Token = token
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        Task<ActionResult<LoginResponse>> IAuthenticationController.PasswordResetAsync(LoginRequest body)
        {
            throw new NotImplementedException();
        }

    }
}
