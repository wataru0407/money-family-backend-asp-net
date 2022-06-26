using MoneyFamily.WebApi.Application.Exceptions;
using MoneyFamily.WebApi.Application.Users;
using MoneyFamily.WebApi.Application.Users.Create;
using MoneyFamily.WebApi.Application.Users.Delete;
using MoneyFamily.WebApi.Application.Users.Get;
using MoneyFamily.WebApi.Application.Users.Login;
using MoneyFamily.WebApi.Application.Users.Query;
using MoneyFamily.WebApi.Application.Users.Update;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using MoneyFamily.WebApi.Domain.Service;
using MoneyFamily.WebApi.Infrastructure.Repository;
using Xunit;

namespace MoneyFamily.WebApi.Tests.Domain.Models.Users
{
    public class UserApplicationServiceTests
    {
        private readonly UserApplicationService _userApplicationService;
        private readonly UserService _userService;
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;
        private readonly Guid test1Id = Guid.Parse("f9450afc-bacb-3468-150c-dd048d37becd");

        public UserApplicationServiceTests()
        {
            _userFactory = new UserFactory();
            _userRepository = new InMemoryUserRepository(_userFactory);
            _userService = new UserService(_userRepository);
            _userApplicationService = new UserApplicationService(_userRepository, _userService, _userFactory);
        }


        [Fact]
        public async void 正常に登録できることを確認する()
        {
            //var userFactory = new UserFactory();
            //var userRepository = new InMemoryUserRepository(userFactory);
            //var userService = new UserService(userRepository);
            //var userApplicationService = new UserApplicationService(userRepository, userService, userFactory);

            var command = new UserCreateCommand("createtest", "create_test@money-family.com", "createtest123");
            var result = await _userApplicationService.CreateUser(command);

            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Email, result.Email);
        }

        [Fact]
        public async void 同じメールアドレスのユーザは登録できないことを確認する()
        {
            //var userFactory = new UserFactory();
            //var userRepository = new InMemoryUserRepository(userFactory);
            //var userService = new UserService(userRepository);
            //var userApplicationService = new UserApplicationService(userRepository, userService, userFactory);

            var command = new UserCreateCommand("test1", "test1@money-family.net", "dummyPassword1");
            var ex = await Assert.ThrowsAsync<CustomDuplicateException>(() => _userApplicationService.CreateUser(command));
            Assert.Equal($"同じメールアドレスのユーザがすでに存在しています。メールアドレス：{command.Email}", ex.Message);
        }

        [Fact]
        public async void 登録されたユーザがログインできることを確認する()
        {
            var command = new UserLoginCommand("test1@money-family.net", "dummyPassword1");
            var result = await _userApplicationService.Login(command);

            Assert.NotNull(result);
        }

        [Fact]
        public async void 登録されていないユーザはログインできないことを確認する()
        {
            var command = new UserLoginCommand("testA@money-family.net", "dummyPassword1");
            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => _userApplicationService.Login(command));
            Assert.Equal($"ユーザが見つかりません。メールアドレス：{command.Email}", ex.Message);
        }

        [Fact]
        public async void パスワードが異なるとログインできないことを確認する()
        {
            var command = new UserLoginCommand("test1@money-family.net", "dummyPassword123");
            var ex = await Assert.ThrowsAsync<CustomCanNotLoginException>(() => _userApplicationService.Login(command));
            Assert.Equal("パスワードが一致しません。", ex.Message);
        }

        [Fact]
        public async void IDでユーザを取得できることを確認する()
        {
            var command = new UserGetCommand(test1Id);
            var result = await _userApplicationService.Get(command);

            Assert.NotNull(result);
            Assert.Equal(command.Id, result.Id);
        }

        [Fact]
        public async void IDでユーザが見つからない場合取得できないことを確認する()
        {
            var command = new UserGetCommand(Guid.NewGuid());
            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => _userApplicationService.Get(command));
            Assert.Equal($"ユーザが見つかりません。ユーザID：{command.Id}", ex.Message);
        }

        [Fact]
        public async void メールアドレスでユーザを取得できることを確認する()
        {
            var command = new UserQueryCommand("test1@money-family.net");
            var result = await _userApplicationService.GetQuery(command);

            Assert.NotNull(result);
            Assert.Equal(command.Email, result.Email);
        }

        [Fact]
        public async void メールアドレスでユーザが見つからない場合取得できないことを確認する()
        {
            var command = new UserQueryCommand("testA@money-family.net");
            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => _userApplicationService.GetQuery(command));
            Assert.Equal($"ユーザが見つかりません。メールアドレスID：{command.Email}", ex.Message);
        }

        [Fact]
        public async void ユーザを更新できることを確認する()
        {
            var command = new UserUpdateCommand(test1Id)
            {
                Name = "test1A",
                Email = "test1A@moneyfamily.net",
                Password = "dummyPassword1A"
            };
            var result = await _userApplicationService.Update(command);
            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Email, result.Email);
        }

        [Fact]
        public async void ユーザが見つからない場合更新できないことを確認する()
        {
            var command = new UserUpdateCommand(Guid.NewGuid()) { Name = "testA" };

            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => _userApplicationService.Update(command));
            Assert.Equal($"ユーザが見つかりません。ユーザID：{command.Id}", ex.Message);
        }

        [Fact]
        public async void 重複するメールアドレスには更新できないことを確認する()
        {
            var command = new UserUpdateCommand(test1Id) { Email = "test2@money-family.net" };

            var ex = await Assert.ThrowsAsync<CustomDuplicateException>(() => _userApplicationService.Update(command));
            Assert.Equal($"同じメールアドレスのユーザがすでに存在しています。メールアドレス：{command.Email}", ex.Message);
        }

        [Fact]
        public async void ユーザを削除できることを確認する()
        {
            var command = new UserDeleteCommand(Guid.Parse("c42361f2-d9ef-557a-43c7-4ed79f3a1a74"));
            await _userApplicationService.Delete(command);

            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => _userApplicationService.Get(new UserGetCommand(Guid.Parse("c42361f2-d9ef-557a-43c7-4ed79f3a1a74"))));

            Assert.Equal($"ユーザが見つかりません。ユーザID：{command.Id}", ex.Message);
        }
    }
}
