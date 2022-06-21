using MoneyFamily.WebApi.Domain.Models.Users;
using Xunit;

namespace MoneyFamily.WebApi.Tests.Domain.Models.Users
{
    public class EmailAddressTests
    {
        [Theory]
        [InlineData("test123@money-family.com")]
        public void 正常にインスタンスが生成できることを確認する(string value)
        {
            var expected = value;
            var userName = new EmailAddress(expected);
            var actual = userName.Value;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void 値がnullや空白の場合に例外が発生することを確認する(string value)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new EmailAddress(value));
            Assert.Equal("Value cannot be null. (Parameter 'value')", ex.Message);
        }

        [Theory]
        [InlineData("test123")]
        [InlineData("test@test@test.com")]
        //[InlineData("1w$")]
        public void メールアドレスの形式が不正の場合に例外が発生することを確認する(string value)
        {
            var ex = Assert.Throws<ArgumentException>(() => new EmailAddress(value));
            Assert.Equal("メールアドレスの形式が不正です。", ex.Message);
        }
    }
}
