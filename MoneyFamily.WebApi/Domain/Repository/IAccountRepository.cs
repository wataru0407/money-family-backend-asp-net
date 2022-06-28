using MoneyFamily.WebApi.Domain.Models.Accounts;
using MoneyFamily.WebApi.Domain.Models.Users;

namespace MoneyFamily.WebApi.Domain.Repository
{
    public interface IAccountRepository
    {
        Task Save(Account account);
        Task Update(Account account);
        Task Delete(Account account);
        Task<Account?> FindById(AccountId id);
        Task<IEnumerable<Account>> GetAll(UserId id);
    }
}
