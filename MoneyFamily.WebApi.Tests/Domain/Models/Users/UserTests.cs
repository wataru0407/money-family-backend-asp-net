using MoneyFamily.WebApi.Domain.Models.Users;
using Xunit;

namespace MoneyFamily.WebApi.Tests.Domain.Models.Users
{
    public class UserTests
    {
        [Fact]
        public void 正常にインスタンスが生成できることを確認する()
        {
            var guid = Guid.NewGuid();
            var id = new UserId(guid);
            var name = new UserName("test");
            var email = new EmailAddress("test@money-family.net");
            var password = new Password("test1234");
            var actual = new User(id, name, email, password);

            Assert.Equal(id, actual.Id);
            Assert.Equal(name, actual.Name);
            Assert.Equal(email, actual.Email);
            Assert.Equal(password, actual.Password);
        }

        [Fact]
        public void プロパティが変更できることを確認する()
        {
            var guid = Guid.NewGuid();
            var id = new UserId(guid);
            var name = new UserName("test");
            var email = new EmailAddress("test@money-family.net");
            var password = new Password("test1234");
            var actual = new User(id, name, email, password);

            var ecpectedName = new UserName("test2");
            var ecpectedEmail = new EmailAddress("test2@money-family.net");
            var expectedPassword = new Password("test12345678");

            actual.ChangeName(ecpectedName);
            actual.ChageEmail(ecpectedEmail);
            actual.ChangePassword(expectedPassword);

            Assert.Equal(ecpectedName, actual.Name);
            Assert.Equal(ecpectedEmail, actual.Email);
            Assert.Equal(expectedPassword, actual.Password);
        }
    }
}
