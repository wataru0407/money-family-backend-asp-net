using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MoneyFamily.WebApi.Application.Accounts;
using MoneyFamily.WebApi.Application.Accounts.GetAll;
using MoneyFamily.WebApi.Application.Exceptions;
using MoneyFamily.WebApi.Controllers;
using System.Security.Claims;

namespace MoneyFamily.WebApi.Presentation.Controllers
{
    public class AccountController : ControllerBase, IAccountsController
    {

        private readonly AccountApplicationService accountApplicationService;
        private IHttpContextAccessor httpContextAccessor;

        public AccountController(AccountApplicationService accountApplicationService, IHttpContextAccessor httpContextAccessor)
        {
            this.accountApplicationService = accountApplicationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ActionResult<ICollection<AccountResponse>>> GetAccountsAsync()
        {
            try
            {
                var accessToken = httpContextAccessor.HttpContext.Request.Headers["Authorization"];

                //var accessToken = Request.Headers[HeaderNames.Authorization];
                var identity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                var id = Guid.Parse(identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var command = new AccountGetCommand(id);
                var result = await accountApplicationService.GetAll(command);
                var response = result.Select(x => {
                    return new AccountResponse() { 
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
            //throw new NotImplementedException();
        }

        public Task<ActionResult<AccountResponse>> AddAccountMemberAsync(AccountMemberRequest body, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<AccountResponse>> CreateAccountAsync(AccountRequest body)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> DeleteAccountAsync(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> DeleteAccountMemberAsync(Guid accountId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<AccountResponse>> GetAccountByIdAsync(Guid accountId)
        {
            throw new NotImplementedException();
        }



        public Task<ActionResult<AccountResponse>> UpdateAccountAsync(AccountRequest body, Guid accountId)
        {
            throw new NotImplementedException();
        }
    }
}
