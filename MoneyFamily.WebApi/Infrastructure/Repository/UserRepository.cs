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
            var result = await moneyFamilyContext.Users.FirstOrDefaultAsync(x => x.EmailAddress == email.Value);
            if (result == null) return null;

            return ToDomainModel(result);
        }

        public async Task<User?> FindById(UserId id)
        {
            var result = await moneyFamilyContext.Users.FirstOrDefaultAsync(x => x.UserId == id.Value);
            if (result == null) return null;

            return ToDomainModel(result);
        }

        public async Task Save(User user)
        {
            moneyFamilyContext.Add(ToDto(user));
            await moneyFamilyContext.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            var dto = ToDto(user);
            var updateUser = await moneyFamilyContext.Users.SingleOrDefaultAsync(x => x.UserId == dto.UserId);
            updateUser.UserName = dto.UserName;
            updateUser.EmailAddress = dto.EmailAddress;
            updateUser.Password = dto.Password;

            await moneyFamilyContext.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            var dto = ToDto(user);
            var deleteUser = await moneyFamilyContext.Users.SingleOrDefaultAsync(x => x.UserId == dto.UserId);
            moneyFamilyContext.Remove(deleteUser);

            await moneyFamilyContext.SaveChangesAsync();
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
                dto.Password);
        }
    }
}
