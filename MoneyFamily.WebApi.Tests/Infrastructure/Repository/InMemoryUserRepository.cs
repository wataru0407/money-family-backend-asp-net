using Microsoft.EntityFrameworkCore;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using MoneyFamily.WebApi.Infrastructure.Models;

namespace MoneyFamily.WebApi.Infrastructure.Repository
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly IUserFactory userFactory;
        private List<UserDto> _users = new List<UserDto>();
        private readonly List<User> userDomains = new List<User>
        {
            new User(new UserId(Guid.Parse("f9450afc-bacb-3468-150c-dd048d37becd")), new UserName("test1"), new EmailAddress("test1@money-family.net"), new Password("dummyPassword1")),
            new User(new UserId(Guid.Parse("6e1607fc-fd61-9600-69ff-e90168ffb367")), new UserName("test2"), new EmailAddress("test2@money-family.net"), new Password("dummyPassword2")),
            new User(new UserId(Guid.Parse("c42361f2-d9ef-557a-43c7-4ed79f3a1a74")), new UserName("test3"), new EmailAddress("test3@money-family.net"), new Password("dummyPassword3")),
        };

        public InMemoryUserRepository(IUserFactory userFactory)
        {
            this.userFactory = userFactory;
            _users = userDomains.Select(x => ToDto(x)).ToList();
        }

        public async Task<User?> FindByEmail(EmailAddress email)
        {
            var result = _users.FirstOrDefault(x => x.EmailAddress.Equals(email.Value));
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
            var result = _users.FirstOrDefault(x => x.UserId.Equals(id.Value));
            if (result == null) return null;
            return ToDomainModel(result);
        }

        public async Task Save(User user)
        {
            _users.Add(ToDto(user));
        }

        public async Task Update(User user)
        {
            var dto = ToDto(user);
            _users.ForEach(x =>
            {
                if (x.UserId == dto.UserId)
                {
                    x.UserName = dto.UserName;
                    x.EmailAddress = dto.EmailAddress;
                    x.Password = dto.Password;
                }
            });
        }

        public async Task Delete(User user)
        {
            var dto = ToDto(user);
            var deleteUser = _users.Find(x => x.UserId == dto.UserId);

            _users.Remove(deleteUser);
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
