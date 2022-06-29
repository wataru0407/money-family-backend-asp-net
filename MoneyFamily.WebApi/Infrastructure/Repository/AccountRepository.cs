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
            //var accountDto = ToAccountDto(account);
            //var updateAccount = await moneyFamilyContext.Accounts.SingleOrDefaultAsync(x => x.AccountId == accountDto.AccountId);
            //updateAccount.AccountName = accountDto.AccountName;
            //await moneyFamilyContext.SaveChangesAsync();

            //var deleteDto = new AccountUserRelationDto();
            //deleteDto.AccountId = account.Id.Value;
            //moneyFamilyContext.AccountUserRelations.AttachRange(deleteDto);
            //moneyFamilyContext.AccountUserRelations.RemoveRange(deleteDto);

            //var addDtos = ToRelationDto(account);
            //moneyFamilyContext.AccountUserRelations.AddRange(addDtos);
            var deleteRelations = await moneyFamilyContext.AccountUserRelations.Where(x => x.AccountId == account.Id.Value).ToListAsync();
            moneyFamilyContext.AccountUserRelations.RemoveRange(deleteRelations);
            moneyFamilyContext.AccountUserRelations.AddRange(ToRelationDto(account));

            await moneyFamilyContext.SaveChangesAsync();
        }

        public async Task Delete(Account account)
        {
            //var accountDto = ToAccountDto(account);
            //var accountDto = new AccountDto() { AccountId = account.Id.Value};

            //moneyFamilyContext.Accounts.Attach(accountDto);
            //moneyFamilyContext.Accounts.Remove(accountDto);

            ////var relationDtos = ToRelationDto(account);
            //var relation = new AccountUserRelationDto();
            //relation.AccountId = account.Id.Value;
            //moneyFamilyContext.AccountUserRelations.AttachRange(relation);
            //moneyFamilyContext.AccountUserRelations.RemoveRange(relation);

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
            //var relationDtos = await (from r in moneyFamilyContext.AccountUserRelations
            //                         where r.AccountId.Equals(id.Value)
            //                         select r).ToListAsync();
            return ToDomainModel(accountDto, relationDtos);
        }

        public async Task<IEnumerable<Account>> GetAll(UserId id)
        {
            var accountIds = await (from r in moneyFamilyContext.AccountUserRelations
                                     where r.UserId.Equals(id.Value)
                                     select r.AccountId).ToListAsync();

            //var accounts = new List<Account>();
            //accountIds.ForEach(x => {
            //    var accountDto = moneyFamilyContext.Accounts.FirstOrDefault(y => y.AccountId.Equals(y.AccountId));
            //    var relationDtos = (from r in moneyFamilyContext.AccountUserRelations
            //                              where r.AccountId.Equals(x)
            //                              select r).ToList();
            //    var result = ToDomainModel(accountDto, relationDtos);
            //    accounts.Add(result);
            //    //return result;
            //});
            var accounts = await Task.WhenAll(accountIds.Select(async x => await FindById(new AccountId(x))));
            //var accounts = accountIds.Select(async x => await FindById(new AccountId(x)));
            //var accounts = accountIds.Select(async x => {
            //    var result = await FindById(new AccountId(x));
            //    return result;
            //    });
            //foreach (var item in accountIds)
            //{
            //    var result = await FindById(new AccountId(item));
            //    accounts.Add(result);
            //}
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
