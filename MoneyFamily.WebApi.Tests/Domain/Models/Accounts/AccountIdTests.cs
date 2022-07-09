using MoneyFamily.WebApi.Domain.Models.Accounts;
using Xunit;

namespace MoneyFamily.WebApi.Tests.Domain.Models.Accounts
{
    public class AccountIdTests
    {
        [Fact]
        public void 正常にインスタンスが生成できることを確認する()
        {
            var expected = Guid.NewGuid();
            var userId = new AccountId(expected);
            var actual = userId.Value;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Guidがnullの場合に例外が発生することを確認する()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new AccountId(Guid.Empty));
            Assert.Equal("Value cannot be null. (Parameter 'value')", ex.Message);
        }
    }
}
