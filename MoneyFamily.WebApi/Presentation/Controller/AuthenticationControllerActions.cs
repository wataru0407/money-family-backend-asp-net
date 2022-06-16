using Microsoft.AspNetCore.Mvc;
using MoneyFamily.WebApi.Application;
using MoneyFamily.WebApi.Application.Service;
using MoneyFamily.WebApi.Controllers;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Presentation.Secutiry;
using System.Net;

namespace MoneyFamily.WebApi.Presentation.Controller
{
    public class AuthenticationControllerActions : ControllerBase, IAuthenticationController
    {
        private readonly UserAppricationService userAppricationService;
        private readonly JwtSetting jwtSetting;

        public AuthenticationControllerActions(UserAppricationService userAppricationService, JwtSetting jwtSetting)
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

        private User ToNewDomainModel(UserRequest request)
        {
            return Domain.Models.Users.User.CreateNew(
                new UserName(request.Name),
                new EmailAddress(request.Email),
                new Password(request.Password));
        }

        private UserResponse ToApiModel(User user)
        {
            return new UserResponse()
            {
                Id = user.Id.Value,
                Name = user.Name.Value,
                Email = user.Email.Value
            };
        }

    }
}
