using MoneyFamily.WebApi.Domain.Models.Users;

namespace MoneyFamily.WebApi.Domain.Repository
{
    public interface IUserRepository
    {
        Task Save(User user);
        Task Update(User user);
        Task<User?> FindById(UserId id);
        Task<User?> FindByEmail(EmailAddress email);
    }
}
