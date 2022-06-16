using MoneyFamily.WebApi.Domain.Models.Users;

namespace MoneyFamily.WebApi.Domain.Repository
{
    public interface IUserRepository
    {
        Task Save(HashUser user);
        Task Update(HashUser user);
        Task<HashUser?> FindById(UserId id);
        Task<HashUser?> FindByEmail(EmailAddress email);
    }
}
