using MoneyFamily.WebApi.Domain.Models.Accounts;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using MoneyFamily.WebApi.Infrastructure.Models;

namespace MoneyFamily.WebApi.Infrastructure.Repository
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        public AccountId AccountId1 { get; set; }
        public AccountId AccountId2 { get; set; }
        public AccountId AccountId3 { get; set; }
        public UserId UserId1 { get; set; }
        public UserId UserId2 { get; set; }
        public UserId UserId3 { get; set; }
        private readonly List<AccountDto> accounts;
        private readonly List<AccountUserRelationDto> accountUserRelations;

        public InMemoryAccountRepository(InMemoryUserRepository userRepository)
        {
            AccountId1 = new AccountId(Guid.NewGuid());
            AccountId2 = new AccountId(Guid.NewGuid());
            AccountId3 = new AccountId(Guid.NewGuid());

            UserId1 = userRepository.userId1;
            UserId2 = userRepository.userId2;
            UserId3 = userRepository.userId3;

            accounts = new List<AccountDto> {
                new AccountDto() { AccountId = AccountId1.Value, AccountName = "test1", CreatedUserId = UserId1.Value },
                new AccountDto() { AccountId = AccountId2.Value, AccountName = "test2", CreatedUserId = UserId2.Value },
                new AccountDto() { AccountId = AccountId3.Value, AccountName = "test3", CreatedUserId = UserId3.Value },
            };

            accountUserRelations = new List<AccountUserRelationDto> {
                new AccountUserRelationDto() { AccountId = AccountId1.Value, UserId = UserId1.Value },
                new AccountUserRelationDto() { AccountId = AccountId2.Value, UserId = UserId1.Value },
                new AccountUserRelationDto() { AccountId = AccountId2.Value, UserId = UserId2.Value },
                new AccountUserRelationDto() { AccountId = AccountId3.Value, UserId = UserId1.Value },
                new AccountUserRelationDto() { AccountId = AccountId3.Value, UserId = UserId2.Value },
                new AccountUserRelationDto() { AccountId = AccountId3.Value, UserId = UserId3.Value },
            };
        }

        public async Task Save(Account account)
        {
            accounts.Add(ToAccountDto(account));
            accountUserRelations.AddRange(ToRelationDto(account));
        }

        public async Task UpdateAccount(Account account)
        {
            var accountDto = ToAccountDto(account);
            var updateAccount = accounts.SingleOrDefault(x => x.AccountId == accountDto.AccountId);
            updateAccount.AccountName = accountDto.AccountName;
        }

        public async Task UpdateMember(Account account)
        {
            accountUserRelations.RemoveAll(x => x.AccountId == account.Id.Value);
            accountUserRelations.AddRange(ToRelationDto(account));
        }

        public async Task Delete(Account account)
        {
            var deleteAccount = accounts.SingleOrDefault(x => x.AccountId == account.Id.Value);
            accounts.Remove(deleteAccount);
            accountUserRelations.RemoveAll(x => x.AccountId == account.Id.Value);
        }

        public async Task<Account?> FindById(AccountId id)
        {
            var accountDto = accounts.FirstOrDefault(x => x.AccountId.Equals(id.Value));
            if (accountDto == null) return null;

            var relationDtos = accountUserRelations.Where(x => x.AccountId == id.Value).ToList();
            return ToDomainModel(accountDto, relationDtos);
        }

        public async Task<IEnumerable<Account>> GetAll(UserId userId)
        {
            var relations = accountUserRelations.Where(x => x.UserId == userId.Value).ToList();
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
            var result = account.GetMembers().Select(x =>
            {
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
