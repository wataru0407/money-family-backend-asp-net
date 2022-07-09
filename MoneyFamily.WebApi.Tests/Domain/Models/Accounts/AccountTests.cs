using MoneyFamily.WebApi.Domain.Models.Accounts;
using MoneyFamily.WebApi.Domain.Models.Users;
using Xunit;

namespace MoneyFamily.WebApi.Tests.Domain.Models.Accounts
{
    public class AccountTests
    {
        [Fact]
        public void 正常に新規作成時のインスタンスが生成できることを確認する()
        {
            var name = new AccountName("test");
            var createUser = new UserId(Guid.NewGuid());
            var actual = Account.CreateNew(name, createUser);

            Assert.Equal(name, actual.Name);
            Assert.Equal(createUser, actual.CreateUser);
            Assert.Equal(createUser, actual.GetMembers().FirstOrDefault());
        }

        [Fact]
        public void 正常にリポジトリから復元時のインスタンスが生成できることを確認する()
        {
            var id = new AccountId(Guid.NewGuid());
            var name = new AccountName("test");
            var createUser = new UserId(Guid.NewGuid());
            var member = new List<UserId>() { createUser };
            var actual = Account.CreateFromRepository(id, name, createUser, member);

            Assert.Equal(id, actual.Id);
            Assert.Equal(name, actual.Name);
            Assert.Equal(createUser, actual.CreateUser);
            Assert.Equal(member, actual.GetMembers());
        }


        [Fact]
        public void プロパティが変更できることを確認する()
        {
            var id = new AccountId(Guid.NewGuid());
            var name = new AccountName("test");
            var createUser = new UserId(Guid.NewGuid());
            var member = new List<UserId>() { createUser };
            var actual = Account.CreateFromRepository(id, name, createUser, member);

            var ecpectedName = new AccountName("test2");

            actual.ChangeName(ecpectedName);

            Assert.Equal(ecpectedName, actual.Name);
        }

        [Fact]
        public void 家計簿にユーザを追加できることを確認する()
        {
            var id = new AccountId(Guid.NewGuid());
            var name = new AccountName("test");
            var createUser = new UserId(Guid.NewGuid());
            var member = new List<UserId>() { createUser };
            var actual = Account.CreateFromRepository(id, name, createUser, member);

            var addUser = new UserId(Guid.NewGuid());
            actual.AddMember(addUser);

            Assert.Contains(addUser, actual.GetMembers());
        }

        [Fact]
        public void 上限を超えた場合家計簿にユーザを追加できないことを確認する()
        {
            var id = new AccountId(Guid.NewGuid());
            var name = new AccountName("test");
            var createUser = new UserId(Guid.NewGuid());
            var member = new List<UserId>() { 
                createUser,
                new UserId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
            };
            var actual = Account.CreateFromRepository(id, name, createUser, member);

            var addUser = new UserId(Guid.NewGuid());
            var ex = Assert.Throws<ArgumentException>(() => actual.AddMember(addUser));
            Assert.Equal("家計簿の人数が上限に達しています。", ex.Message);
        }

        [Fact]
        public void すでに家計簿に追加されているユーザは追加できないことを確認する()
        {
            var id = new AccountId(Guid.NewGuid());
            var name = new AccountName("test");
            var createUser = new UserId(Guid.NewGuid());
            var member = new List<UserId>() {
                createUser,
                new UserId(Guid.NewGuid()),
            };
            var actual = Account.CreateFromRepository(id, name, createUser, member);

            var ex = Assert.Throws<ArgumentException>(() => actual.AddMember(createUser));
            Assert.Equal("すでに追加されているユーザです。", ex.Message);
        }

        [Fact]
        public void 家計簿からユーザを削除できることを確認する()
        {
            var id = new AccountId(Guid.NewGuid());
            var name = new AccountName("test");
            var createUser = new UserId(Guid.NewGuid());
            var deleteUser = new UserId(Guid.NewGuid());
            var member = new List<UserId>() { createUser, deleteUser };
            var actual = Account.CreateFromRepository(id, name, createUser, member);

            actual.RemoveMember(deleteUser);

            Assert.DoesNotContain(deleteUser, actual.GetMembers());
        }

        [Fact]
        public void 家計簿の作成者はユーザから削除できないことを確認する()
        {
            var id = new AccountId(Guid.NewGuid());
            var name = new AccountName("test");
            var createUser = new UserId(Guid.NewGuid());
            var member = new List<UserId>() {
                createUser,
                new UserId(Guid.NewGuid()),
                new UserId(Guid.NewGuid())
            };
            var actual = Account.CreateFromRepository(id, name, createUser, member);

            var ex = Assert.Throws<ArgumentException>(() => actual.RemoveMember(createUser));
            Assert.Equal("家計簿の作成者は削除できません。", ex.Message);
        }
    }
}
