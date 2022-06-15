using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;

namespace MoneyFamily.WebApi.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MoneyFamilyContext moneyFamilyContext;

        public UserRepository(MoneyFamilyContext moneyFamilyContext)
        {
            this.moneyFamilyContext = moneyFamilyContext;
        }

        public async Task<User?> FindByEmail(EmailAddress email)
        {
            var test = moneyFamilyContext.Users;
            var emailad = email.Value;
            var result = moneyFamilyContext.Users.FirstOrDefault(x => x.EmailAddress.Equals(email.Value));
            if (result == null) return null;
            var model = User.CreateFromRepository(
                    new UserId(result.UserId),
                    new UserName(result.UserName),
                    new EmailAddress(result.EmailAddress),
                    new Password(result.Password)
                );

            return model;

            // 仮生成
            return User.CreateFromRepository(
                new UserId(Guid.Parse("f29e6562-5105-723e-b799-340bfbcfaa79")),
                new UserName("admin"),
                new EmailAddress("admin@gmail.com"),
                new Password("admin1234")
                );
            //throw new NotImplementedException();
        }

        public async Task<User?> FindById(UserId id)
        {

            // 仮生成
            return User.CreateFromRepository(
                new UserId(Guid.Parse("f29e6562-5105-723e-b799-340bfbcfaa79")),
                new UserName("admin"),
                new EmailAddress("admin@gmail.com"),
                new Password("admin")
                );
            //throw new NotImplementedException();
        }

        Task IUserRepository.Save(User user)
        {
            throw new NotImplementedException();
        }

        Task IUserRepository.Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
