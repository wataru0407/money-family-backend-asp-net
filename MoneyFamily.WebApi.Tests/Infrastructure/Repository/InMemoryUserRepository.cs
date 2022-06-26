using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using MoneyFamily.WebApi.Infrastructure.Models;

namespace MoneyFamily.WebApi.Infrastructure.Repository
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly IUserFactory userFactory;
        private readonly List<UserDto> userDtos = new List<UserDto>();
        private readonly List<User> users = new List<User>
        {
            new User(new UserId(Guid.Parse("f9450afc-bacb-3468-150c-dd048d37becd")), new UserName("test1"), new EmailAddress("test1@money-family.net"), new Password("dummyPassword1")),
            new User(new UserId(Guid.Parse("6e1607fc-fd61-9600-69ff-e90168ffb367")), new UserName("test2"), new EmailAddress("test2@money-family.net"), new Password("dummyPassword2")),
            new User(new UserId(Guid.Parse("c42361f2-d9ef-557a-43c7-4ed79f3a1a74")), new UserName("test3"), new EmailAddress("test3@money-family.net"), new Password("dummyPassword3")),
        };

        public InMemoryUserRepository(IUserFactory userFactory)
        {
            this.userFactory = userFactory;
            this.userDtos = users.Select(x => ToDto(x)).ToList();
        }

        public async Task<User?> FindByEmail(EmailAddress email)
        {
            var result = userDtos.FirstOrDefault(x => x.EmailAddress.Equals(email.Value));
            if (result == null) return null;
            return ToDomainModel(result);
        }

        public async Task<User?> FindById(UserId id)
        {
            var result = userDtos.FirstOrDefault(x => x.UserId.Equals(id.Value));
            if (result == null) return null;
            return ToDomainModel(result);
        }

        public async Task Save(User user)
        {
            userDtos.Add(ToDto(user));
        }

        public async Task Update(User user)
        {
            var dto = ToDto(user);
            userDtos.ForEach(x =>
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
            var deleteUser = userDtos.Find(x => x.UserId == dto.UserId);
            userDtos.Remove(deleteUser);
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
