using Microsoft.AspNetCore.Mvc;
using MoneyFamily.WebApi.Application.Exceptions;
using MoneyFamily.WebApi.Application.Users;
using MoneyFamily.WebApi.Application.Users.Create;
using MoneyFamily.WebApi.Application.Users.Delete;
using MoneyFamily.WebApi.Application.Users.Get;
using MoneyFamily.WebApi.Application.Users.Query;
using MoneyFamily.WebApi.Application.Users.Update;
using MoneyFamily.WebApi.Controllers;

namespace MoneyFamily.WebApi.Presentation.Controller
{
    public class UserController : ControllerBase, IUsersController
    {
        private readonly UserAppricationService userAppricationService;

        public UserController(UserAppricationService userAppricationService)
        {
            this.userAppricationService = userAppricationService;
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

        public async Task<IActionResult> DeleteUserByIdAsync(Guid userId)
        {
            try
            {
                var command = new UserDeleteCommand(userId);
                await userAppricationService.Delete(command);
                return NoContent();
            }
            catch (CustomNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        public async Task<ActionResult<UserResponse>> GetUserByEmailAddressAsync(string emailAddress)
        {
            try
            {
                var command = new UserQueryCommand(emailAddress);
                var result = await userAppricationService.GetQuery(command);
                var response = new UserResponse()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Email = result.Email
                };
                return Ok(response);
            }
            catch (CustomNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<ActionResult<UserResponse>> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var command = new UserGetCommand(userId);
                var result = await userAppricationService.Get(command);
                var response = new UserResponse()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Email = result.Email
                };
                return Ok(response);
            }
            catch (CustomNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public async Task<ActionResult<UserResponse>> UpdateUserByIdAsync(UserRequest body, Guid userId)
        {
            if (body == null) return BadRequest();

            try
            {
                var command = new UserUpdateCommand(userId)
                {
                    Name = body.Name,
                    Email = body.Email,
                    Password = body.Password
                };
                var result = await userAppricationService.Update(command);
                var response = new UserResponse()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Email = result.Email
                };
                return Ok(response);
            }
            catch (CustomNotFoundException ex)
            {
                return NotFound(ex.Message);
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
    }
}
