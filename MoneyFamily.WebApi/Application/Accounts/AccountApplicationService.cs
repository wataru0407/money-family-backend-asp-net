using MoneyFamily.WebApi.Application.Accounts.GetAll;
using MoneyFamily.WebApi.Application.Exceptions;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;

namespace MoneyFamily.WebApi.Application.Accounts
{
    public class AccountApplicationService
    {
        private readonly IAccountRepository accountRepository;
        public AccountApplicationService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<List<AccountGetResult>> GetAll(AccountGetCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var id = new UserId(command.UserId);
            var accounts = await accountRepository.GetAll(id);
            if (accounts == null) throw new CustomNotFoundException($"ユーザが所属する家計簿が見つかりません。ユーザID：{command.UserId}");

            var result = accounts.Select(x => {
                return new AccountGetResult(
                    x.Id.Value,
                    x.Name.Value,
                    x.CreateUser.Value,
                    x.GetMembers().Select(x => x.Value).ToList());
            });
            return result.ToList();
        }
    }
}
