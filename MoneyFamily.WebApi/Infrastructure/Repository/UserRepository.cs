using Microsoft.EntityFrameworkCore;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using MoneyFamily.WebApi.Infrastructure.Models;

namespace MoneyFamily.WebApi.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MoneyFamilyContext moneyFamilyContext;
        private readonly IUserFactory userFactory;

        public UserRepository(MoneyFamilyContext moneyFamilyContext, IUserFactory userFactory)
        {
            this.moneyFamilyContext = moneyFamilyContext;
            this.userFactory = userFactory;
        }

        public async Task<User?> FindByEmail(EmailAddress email)
        {
            //var test = moneyFamilyContext.Users;
            //var emailad = email.Value;
            var result = await moneyFamilyContext.Users.FirstOrDefaultAsync(x => x.EmailAddress.Equals(email.Value));
            if (result == null) return null;
            return ToDomainModel(result);

            // 仮生成
            //return User.CreateFromRepository(
            //    new UserId(Guid.Parse("f29e6562-5105-723e-b799-340bfbcfaa79")),
            //    new UserName("admin"),
            //    new EmailAddress("admin@gmail.com"),
            //    new Password("admin1234")
            //    );
            //throw new NotImplementedException();
        }

        public async Task<User?> FindById(UserId id)
        {
            var result = await moneyFamilyContext.Users.FirstOrDefaultAsync(x => x.UserId.Equals(id.Value));
            if (result == null) return null;
            return ToDomainModel(result);

            // 仮生成
            //return User.CreateInstance(
            //    new UserId(Guid.Parse("f29e6562-5105-723e-b799-340bfbcfaa79")),
            //    new UserName("admin"),
            //    new EmailAddress("admin@gmail.com"),
            //    new Password("admin")
            //    );
            //throw new NotImplementedException();
        }

        public async Task Save(User user)
        {

            moneyFamilyContext.Add(ToDto(user));
            await moneyFamilyContext.SaveChangesAsync();
            //throw new NotImplementedException();
        }

        public async Task Update(User user)
        {
            moneyFamilyContext.Entry(ToDto(user)).State = EntityState.Modified;
            //throw new NotImplementedException();
        }

        private static UserDto ToDto(User user)
        {
            return new UserDto()
            {
                UserId = user.Id.Value,
                UserName = user.Name.Value,
                EmailAddress = user.Email.Value,
                Password = user.HashPassword
            };
        }

        private User ToDomainModel(UserDto dto)
        {
            return userFactory.CreateFromRepository(
                new UserId(dto.UserId),
                new UserName(dto.UserName),
                new EmailAddress(dto.EmailAddress),
                dto.Password
                );
        }
    }
}
