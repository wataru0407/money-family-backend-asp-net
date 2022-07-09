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

namespace MoneyFamily.WebApi.Tests.Application.Users
{
    public class UserApplicationServiceTests
    {
        private readonly UserApplicationService userApplicationService;
        private readonly UserService userService;
        private readonly IUserFactory userFactory;
        private readonly InMemoryUserRepository userRepository;

        public UserApplicationServiceTests()
        {
            userFactory = new UserFactory();
            userRepository = new InMemoryUserRepository(userFactory);
            userService = new UserService(userRepository);
            userApplicationService = new UserApplicationService(userFactory, userRepository, userService);
        }

        [Fact]
        public async void 登録されたユーザはログインできることを確認する()
        {
            var command = new UserLoginCommand("test1@money-family.net", "dummyPassword1");
            var result = await userApplicationService.Login(command);

            Assert.NotNull(result);
        }

        [Fact]
        public async void 登録されていないユーザはログインできないことを確認する()
        {
            var command = new UserLoginCommand("testA@money-family.net", "dummyPassword1");
            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => userApplicationService.Login(command));

            Assert.Equal($"ユーザが見つかりません。メールアドレス：{command.Email}", ex.Message);
        }

        [Fact]
        public async void パスワードが異なるとログインできないことを確認する()
        {
            var command = new UserLoginCommand("test1@money-family.net", "dummyPassword123");
            var ex = await Assert.ThrowsAsync<CustomCanNotLoginException>(() => userApplicationService.Login(command));
            Assert.Equal("パスワードが一致しません。", ex.Message);
        }


        [Fact]
        public async void ユーザを正常に登録できることを確認する()
        {
            var command = new UserCreateCommand("test4", "test4@money-family.com", "dummyPassword4");
            var result = await userApplicationService.Create(command);

            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Email, result.Email);
        }

        [Fact]
        public async void 同じメールアドレスのユーザは登録できないことを確認する()
        {
            var command = new UserCreateCommand("test1", "test1@money-family.net", "dummyPassword1");
            var ex = await Assert.ThrowsAsync<CustomDuplicateException>(() => userApplicationService.Create(command));

            Assert.Equal($"同じメールアドレスのユーザがすでに存在しています。メールアドレス：{command.Email}", ex.Message);
        }

        [Fact]
        public async void ユーザを取得できることを確認する()
        {
            var command = new UserGetCommand(userRepository.userId1.Value);
            var result = await userApplicationService.Get(command);

            Assert.NotNull(result);
            Assert.Equal(command.Id, result.Id);
        }

        [Fact]
        public async void ユーザが見つからない場合取得できないことを確認する()
        {
            var command = new UserGetCommand(Guid.NewGuid());
            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => userApplicationService.Get(command));

            Assert.Equal($"ユーザが見つかりません。ユーザID：{command.Id}", ex.Message);
        }

        [Fact]
        public async void メールアドレスでユーザを取得できることを確認する()
        {
            var command = new UserQueryCommand("test1@money-family.net");
            var result = await userApplicationService.GetQuery(command);

            Assert.NotNull(result);
            Assert.Equal(command.Email, result.Email);
        }

        [Fact]
        public async void メールアドレスでユーザが見つからない場合取得できないことを確認する()
        {
            var command = new UserQueryCommand("testA@money-family.net");
            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => userApplicationService.GetQuery(command));
            Assert.Equal($"ユーザが見つかりません。メールアドレスID：{command.Email}", ex.Message);
        }

        [Fact]
        public async void ユーザを更新できることを確認する()
        {
            var command = new UserUpdateCommand(userRepository.userId2.Value)
            {
                Name = "test2A",
                Email = "test2A@moneyfamily.net",
                Password = "dummyPassword2A"
            };
            var result = await userApplicationService.Update(command);

            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Email, result.Email);
        }

        [Fact]
        public async void ユーザが見つからない場合更新できないことを確認する()
        {
            var command = new UserUpdateCommand(Guid.NewGuid()) { Name = "testA" };

            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => userApplicationService.Update(command));
            Assert.Equal($"ユーザが見つかりません。ユーザID：{command.Id}", ex.Message);
        }

        [Fact]
        public async void 重複するメールアドレスには更新できないことを確認する()
        {
            var command = new UserUpdateCommand(userRepository.userId2.Value) { Email = "test1@money-family.net" };

            var ex = await Assert.ThrowsAsync<CustomDuplicateException>(() => userApplicationService.Update(command));
            Assert.Equal($"同じメールアドレスのユーザがすでに存在しています。メールアドレス：{command.Email}", ex.Message);
        }

        [Fact]
        public async void ユーザを削除できることを確認する()
        {
            var command = new UserDeleteCommand(userRepository.userId3.Value);
            await userApplicationService.Delete(command);

            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => userApplicationService.Get(new UserGetCommand(userRepository.userId3.Value)));

            Assert.Equal($"ユーザが見つかりません。ユーザID：{command.Id}", ex.Message);
        }
    }
}
