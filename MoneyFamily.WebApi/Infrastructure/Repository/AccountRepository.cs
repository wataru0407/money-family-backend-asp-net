using Microsoft.EntityFrameworkCore;
using MoneyFamily.WebApi.Domain.Models.Accounts;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using MoneyFamily.WebApi.Infrastructure.Models;

namespace MoneyFamily.WebApi.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MoneyFamilyContext moneyFamilyContext;

        public AccountRepository(MoneyFamilyContext moneyFamilyContext)
        {
            this.moneyFamilyContext = moneyFamilyContext;
        }

        public async Task Save(Account account)
        {
            moneyFamilyContext.Accounts.Add(ToAccountDto(account));
            moneyFamilyContext.AccountUserRelations.AddRange(ToRelationDto(account));

            await moneyFamilyContext.SaveChangesAsync();
        }

        public async Task UpdateAccount(Account account)
        {
            var accountDto = ToAccountDto(account);
            var updateAccount = await moneyFamilyContext.Accounts.SingleOrDefaultAsync(x => x.AccountId == accountDto.AccountId);
            updateAccount.AccountName = accountDto.AccountName;

            await moneyFamilyContext.SaveChangesAsync();
        }

        public async Task UpdateMember(Account account)
        {
            var deleteRelations = await moneyFamilyContext.AccountUserRelations.Where(x => x.AccountId == account.Id.Value).ToListAsync();
            moneyFamilyContext.AccountUserRelations.RemoveRange(deleteRelations);
            var addRelations = ToRelationDto(account);
            moneyFamilyContext.AccountUserRelations.AddRange(addRelations);

            await moneyFamilyContext.SaveChangesAsync();
        }

        public async Task Delete(Account account)
        {
            var deleteAccount = await moneyFamilyContext.Accounts.SingleOrDefaultAsync(x => x.AccountId == account.Id.Value);
            moneyFamilyContext.Accounts.Remove(deleteAccount);
            var deleteRelations = await moneyFamilyContext.AccountUserRelations.Where(x => x.AccountId == account.Id.Value).ToListAsync();
            moneyFamilyContext.AccountUserRelations.RemoveRange(deleteRelations);

            await moneyFamilyContext.SaveChangesAsync();
        }

        public async Task<Account?> FindById(AccountId id)
        {
            var accountDto = await moneyFamilyContext.Accounts.FirstOrDefaultAsync(x => x.AccountId.Equals(id.Value));
            if (accountDto == null) return null;

            var relationDtos = await moneyFamilyContext.AccountUserRelations.Where(x => x.AccountId == id.Value).ToListAsync();
            return ToDomainModel(accountDto, relationDtos);
        }

        public async Task<IEnumerable<Account>> GetAll(UserId userId)
        {
            var relations = await moneyFamilyContext.AccountUserRelations.Where(x => x.UserId == userId.Value).ToListAsync();
            var accounts = new List<Account>();
            foreach (var relation in relations)
            {
                var id = new AccountId(relation.AccountId);
                var account = await FindById(id);
                if (account is null) continue;
                accounts.Add(account);
            }
            return accounts;
        }


        private static AccountDto ToAccountDto(Account account)
        {
            return new AccountDto()
            {
                AccountId = account.Id.Value,
                AccountName = account.Name.Value,
                CreatedUserId = account.CreateUser.Value
            };
        }

        private static IEnumerable<AccountUserRelationDto> ToRelationDto(Account account)
        {
            var result = account.GetMembers().Select(x => {
                return new AccountUserRelationDto()
                {
                    AccountId = account.Id.Value,
                    UserId = x.Value,
                };
            });
            return result;
        }

        private static Account ToDomainModel(AccountDto accountDto, IEnumerable<AccountUserRelationDto> relationDtos)
        {
            var members = relationDtos.Select(x => new UserId(x.UserId));
            return Account.CreateFromRepository(
                new AccountId(accountDto.AccountId),
                new AccountName(accountDto.AccountName),
                new UserId(accountDto.CreatedUserId),
                members.ToList());
        }
    }
}
