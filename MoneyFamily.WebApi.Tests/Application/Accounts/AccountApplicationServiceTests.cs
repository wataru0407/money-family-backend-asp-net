using MoneyFamily.WebApi.Application.Accounts;
using MoneyFamily.WebApi.Application.Accounts.AddMember;
using MoneyFamily.WebApi.Application.Accounts.Create;
using MoneyFamily.WebApi.Application.Accounts.Delete;
using MoneyFamily.WebApi.Application.Accounts.Get;
using MoneyFamily.WebApi.Application.Accounts.GetAll;
using MoneyFamily.WebApi.Application.Accounts.RemoveMember;
using MoneyFamily.WebApi.Application.Accounts.Update;
using MoneyFamily.WebApi.Application.Exceptions;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Infrastructure.Repository;
using Xunit;

namespace MoneyFamily.WebApi.Tests.Application.Accounts
{
    public class AccountApplicationServiceTests
    {
        private readonly AccountApplicationService accountApplicationService;
        private readonly IUserFactory userFactory;
        private readonly InMemoryAccountRepository accountRepository;
        private readonly InMemoryUserRepository userRepository;


        public AccountApplicationServiceTests()
        {
            userFactory = new UserFactory();
            userRepository = new InMemoryUserRepository(userFactory);
            accountRepository = new InMemoryAccountRepository(userRepository);
            accountApplicationService = new AccountApplicationService(accountRepository, userRepository);
        }

        [Fact]
        public async void 家計簿を正常に登録できることを確認する()
        {
            var createUserId = Guid.NewGuid();
            var command = new AccountCreateCommand("test4", createUserId);
            var result = await accountApplicationService.Create(command);

            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.CreateUserId, result.CreateUserId);
            Assert.Contains(createUserId, result.Members);
        }

        [Fact]
        public async void 家計簿を取得できることを確認する()
        {
            var command = new AccountGetCommand(accountRepository.AccountId1.Value);
            var result = await accountApplicationService.Get(command);

            Assert.NotNull(result);
            Assert.Equal(command.Id, result.Id);
        }

        [Fact]
        public async void 家計簿が見つからない場合取得できないことを確認する()
        {
            var command = new AccountGetCommand(Guid.NewGuid());
            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => accountApplicationService.Get(command));

            Assert.Equal($"家計簿が見つかりません。家計簿ID：{command.Id}", ex.Message);
        }

        [Fact]
        public async void ユーザの所属する家計簿の一覧が取得できることを確認する()
        {
            var command = new AccountGetAllCommand(accountRepository.UserId1.Value);
            var result = await accountApplicationService.GetAll(command);

            foreach(var item in result)
            {
                Assert.NotNull(item);
                Assert.Contains(command.UserId, item.Members);
            }
        }

        [Fact]
        public async void ユーザの所属する家計簿が見つからない場合取得できないことを確認する()
        {
            var command = new AccountGetAllCommand(Guid.NewGuid());
            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => accountApplicationService.GetAll(command));
            Assert.Equal($"ユーザが所属する家計簿が見つかりません。ユーザID：{command.UserId}", ex.Message);
        }

        [Fact]
        public async void 家計簿を更新できることを確認する()
        {
            var command = new AccountUpdateCommand(accountRepository.AccountId2.Value, "test2A");
            var result = await accountApplicationService.Update(command);

            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
        }

        [Fact]
        public async void 家計簿が見つからない場合更新できないことを確認する()
        {
            var command = new AccountUpdateCommand(Guid.NewGuid(), "testA");

            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => accountApplicationService.Update(command));
            Assert.Equal($"家計簿が見つかりません。家計簿ID：{command.Id}", ex.Message);
        }

        [Fact]
        public async void 家計簿を削除できることを確認する()
        {
            var command = new AccountDeleteCommand(accountRepository.AccountId3.Value, accountRepository.UserId3.Value);
            await accountApplicationService.Delete(command);

            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => accountApplicationService.Get(new AccountGetCommand(accountRepository.AccountId3.Value)));

            Assert.Equal($"家計簿が見つかりません。家計簿ID：{command.Id}", ex.Message);
        }

        [Fact]
        public async void 家計簿が見つからない場合削除できないことを確認する()
        {
            var command = new AccountDeleteCommand(Guid.NewGuid(), Guid.NewGuid());

            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => accountApplicationService.Delete(command));
            Assert.Equal($"家計簿が見つかりません。家計簿ID：{command.Id}", ex.Message);
        }

        [Fact]
        public async void 家計簿の作成者でない場合削除できないことを確認する()
        {
            var command = new AccountDeleteCommand(accountRepository.AccountId1.Value, accountRepository.UserId3.Value);

            var ex = await Assert.ThrowsAsync<CustomCanNotDeleteException>(() => accountApplicationService.Delete(command));
            Assert.Equal("家計簿の作成者以外は削除できません。", ex.Message);
        }

        [Fact]
        public async void 家計簿にユーザを追加できることを確認する()
        {
            var addUserId = accountRepository.UserId2.Value;
            var command = new AccountAddMemberCommand(accountRepository.AccountId1.Value, addUserId);
            var result = await accountApplicationService.AddMember(command);

            Assert.NotNull(result);
            Assert.Contains(addUserId, result.Members);
        }

        [Fact]
        public async void 追加するユーザが見つからない場合追加できないことを確認する()
        {
            var addUserId = Guid.NewGuid();
            var command = new AccountAddMemberCommand(accountRepository.AccountId1.Value, addUserId);

            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => accountApplicationService.AddMember(command));
            Assert.Equal($"追加するユーザが見つかりません。ユーザID：{command.UserId}", ex.Message);
        }

        [Fact]
        public async void 家計簿からユーザを削除できることを確認する()
        {
            var accountId = accountRepository.AccountId2.Value;
            var removeUserId = accountRepository.UserId1.Value;
            var command = new AccountRemoveMemberCommand(accountId, removeUserId);
            await accountApplicationService.RemoveMember(command);

            var result = await accountApplicationService.Get(new AccountGetCommand(accountId));

            Assert.DoesNotContain(removeUserId, result.Members);
        }

        [Fact]
        public async void 削除するユーザが見つからない場合削除できないことを確認する()
        {
            var removeUserId = Guid.NewGuid();
            var command = new AccountRemoveMemberCommand(accountRepository.AccountId2.Value, removeUserId);

            var ex = await Assert.ThrowsAsync<CustomNotFoundException>(() => accountApplicationService.RemoveMember(command));
            Assert.Equal($"削除するユーザが見つかりません。ユーザID：{command.UserId}", ex.Message);
        }
    }
}
