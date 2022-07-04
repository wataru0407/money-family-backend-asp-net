using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MoneyFamily.WebApi.Application.Accounts;
using MoneyFamily.WebApi.Application.Accounts.AddMember;
using MoneyFamily.WebApi.Application.Accounts.Create;
using MoneyFamily.WebApi.Application.Accounts.Delete;
using MoneyFamily.WebApi.Application.Accounts.Get;
using MoneyFamily.WebApi.Application.Accounts.GetAll;
using MoneyFamily.WebApi.Application.Accounts.RemoveMember;
using MoneyFamily.WebApi.Application.Accounts.Update;
using MoneyFamily.WebApi.Application.Exceptions;
using MoneyFamily.WebApi.Controllers;
using System.Security.Claims;

namespace MoneyFamily.WebApi.Presentation.Controllers
{
    public class AccountController : ControllerBase, IAccountsController
    {

        private readonly AccountApplicationService accountApplicationService;
        private readonly Guid userId;

        public AccountController(AccountApplicationService accountApplicationService, IHttpContextAccessor httpContextAccessor)
        {
            this.accountApplicationService = accountApplicationService;

            var identity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            userId = Guid.Parse(identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }

        public async Task<ActionResult<ICollection<AccountResponse>>> GetAccountsAsync()
        {
            try
            {
                var command = new AccountGetAllCommand(userId);
                var result = await accountApplicationService.GetAll(command);
                var response = result.Select(x =>
                {
                    return new AccountResponse()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CreateUserId = x.CreateUserId,
                        Members = x.Members,
                    };
                });
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

        public async Task<ActionResult<AccountResponse>> AddAccountMemberAsync(AccountMemberRequest body, Guid accountId)
        {
            if (body == null) return BadRequest(nameof(body));

            try
            {
                var command = new AccountAddMemberCommand(accountId, body.UserId);
                var result = await accountApplicationService.AddMember(command);
                var response = new AccountResponse()
                {
                    Id = result.Id,
                    Name = result.Name,
                    CreateUserId = result.CreateUserId,
                    Members = result.Members,
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

        public async Task<ActionResult<AccountResponse>> CreateAccountAsync(AccountRequest body)
        {
            try
            {
                var command = new AccountCreateCommand(body.Name, userId);
                var result = await accountApplicationService.Create(command);
                var response = new AccountResponse()
                {
                    Id = result.Id,
                    Name = result.Name,
                    CreateUserId = result.CreateUserId,
                    Members = result.Members,
                };
                return Created(response.Id.ToString(), response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> DeleteAccountAsync(Guid accountId)
        {
            try
            {
                var command = new AccountDeleteCommand(accountId, userId);
                await accountApplicationService.Delete(command);
                return NoContent();
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

        public async Task<IActionResult> DeleteAccountMemberAsync(Guid accountId, Guid userId)
        {
            try
            {
                var command = new AccountRemoveMemberCommand(accountId, userId);
                await accountApplicationService.RemoveMember(command);
                return NoContent();
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

        public async Task<ActionResult<AccountResponse>> GetAccountByIdAsync(Guid accountId)
        {
            try
            {
                var command = new AccountGetCommand(accountId);
                var result = await accountApplicationService.Get(command);
                var response = new AccountResponse()
                {
                    Id = result.Id,
                    Name = result.Name,
                    CreateUserId= result.CreateUserId,
                    Members= result.Members,
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



        public async Task<ActionResult<AccountResponse>> UpdateAccountAsync(AccountRequest body, Guid accountId)
        {
            if (body == null) return BadRequest(nameof(body));

            try
            {
                var command = new AccountUpdateCommand(accountId, body.Name);
                var result = await accountApplicationService.Update(command);
                var response = new AccountResponse()
                {
                    Id = result.Id,
                    Name = result.Name,
                    CreateUserId = result.CreateUserId,
                    Members = result.Members,
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
    }
}
