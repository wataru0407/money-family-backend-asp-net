using MoneyFamily.WebApi.Application.Accounts.AddMember;
using MoneyFamily.WebApi.Application.Accounts.Create;
using MoneyFamily.WebApi.Application.Accounts.Delete;
using MoneyFamily.WebApi.Application.Accounts.Get;
using MoneyFamily.WebApi.Application.Accounts.GetAll;
using MoneyFamily.WebApi.Application.Accounts.RemoveMember;
using MoneyFamily.WebApi.Application.Accounts.Update;
using MoneyFamily.WebApi.Application.Exceptions;
using MoneyFamily.WebApi.Domain.Models.Accounts;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using System.Linq;
using System.Transactions;

namespace MoneyFamily.WebApi.Application.Accounts
{
    public class AccountApplicationService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IUserRepository userRepository;
        public AccountApplicationService(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            this.accountRepository = accountRepository;
            this.userRepository = userRepository;
        }

        public async Task<List<AccountGetAllResult>> GetAll(AccountGetAllCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var id = new UserId(command.UserId);
            var accounts = await accountRepository.GetAll(id);
            if (accounts == null) throw new CustomNotFoundException($"ユーザが所属する家計簿が見つかりません。ユーザID：{command.UserId}");

            var result = accounts.Select(x =>
            {
                return new AccountGetAllResult(
                    x.Id.Value,
                    x.Name.Value,
                    x.CreateUser.Value,
                    x.GetMemberIds());
            });
            return result.ToList();
        }

        public async Task<AccountCreateResult> Create(AccountCreateCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var account = Account.CreateNew(
                new AccountName(command.Name),
                new UserId(command.CreateUserId));

            await accountRepository.Save(account);
            var result = await accountRepository.FindById(account.Id);

            transaction.Complete();

            return new AccountCreateResult(
                result.Id.Value,
                result.Name.Value,
                result.CreateUser.Value,
                result.GetMemberIds());
        }

        public async Task<AccountGetResult> Get(AccountGetCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var id = new AccountId(command.Id);
            var account = await accountRepository.FindById(id);
            if (account == null) throw new CustomNotFoundException($"家計簿が見つかりません。家計簿ID：{command.Id}");

            return new AccountGetResult(account.Id.Value, account.Name.Value, account.CreateUser.Value, account.GetMemberIds());
        }

        public async Task<AccountUpdateResult> Update(AccountUpdateCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var id = new AccountId(command.Id);
            var account = await accountRepository.FindById(id);
            if (account == null) throw new CustomNotFoundException($"家計簿が見つかりません。家計簿ID：{command.Id}");

            if (command.Name is not null)
            {
                var name = new AccountName(command.Name);
                account.ChangeName(name);
            }

            await accountRepository.UpdateAccount(account);
            var result = await accountRepository.FindById(id);

            transaction.Complete();

            return new AccountUpdateResult(result.Id.Value, result.Name.Value, result.CreateUser.Value, result.GetMemberIds());
        }

        public async Task Delete(AccountDeleteCommand command)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var id = new AccountId(command.Id);
            var account = await accountRepository.FindById(id);
            if (account == null) throw new CustomNotFoundException($"家計簿が見つかりません。家計簿ID：{command.Id}");

            if (account.CreateUser.Value != command.UserId) throw new CustomCanNotDeleteException("家計簿の作成者以外は削除できません。");

            await accountRepository.Delete(account);

            transaction.Complete();
        }

        public async Task<AccountAddMemberResult> AddMember(AccountAddMemberCommand command)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var id = new AccountId(command.Id);
            var account = await accountRepository.FindById(id);
            if (account == null) throw new CustomNotFoundException($"家計簿が見つかりません。家計簿ID：{command.Id}");

            var userId = new UserId(command.UserId);
            var user = await userRepository.FindById(userId);
            if (user == null) throw new CustomNotFoundException($"追加するユーザが見つかりません。ユーザID：{command.UserId}");

            account.AddMember(userId);
            await accountRepository.UpdateMember(account);
            var result = await accountRepository.FindById(id);

            transaction.Complete();

            return new AccountAddMemberResult(result.Id.Value, result.Name.Value, result.CreateUser.Value, result.GetMemberIds());
        }

        public async Task RemoveMember(AccountRemoveMemberCommand command)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var id = new AccountId(command.Id);
            var account = await accountRepository.FindById(id);
            if (account == null) throw new CustomNotFoundException($"家計簿が見つかりません。家計簿ID：{command.Id}");

            var userId = new UserId(command.UserId);
            var user = await userRepository.FindById(userId);
            if (user == null) throw new CustomNotFoundException($"削除するユーザが見つかりません。ユーザID：{command.UserId}");

            account.RemoveMember(userId);
            await accountRepository.UpdateMember(account);
            var result = await accountRepository.FindById(id);

            transaction.Complete();
        }

    }
}
