using MoneyFamily.WebApi.Domain.Models.Users;
using Xunit;

namespace MoneyFamily.WebApi.Tests.Domain.Models.Users
{
    public class PasswordTests
    {
        [Theory]
        [InlineData("test1234")]
        public void 正常にインスタンスが生成できることを確認する(string value)
        {
            var expected = value;
            var userName = new Password(expected);
            var actual = userName.Value;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void 値がnullや空白の場合に例外が発生することを確認する(string value)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new Password(value));
            Assert.Equal("Value cannot be null. (Parameter 'value')", ex.Message);
        }

        [Theory]
        [InlineData("test123")]
        [InlineData("test123456789012345678901234567890")]
        public void 文字数が不正の場合に例外が発生することを確認する(string value)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Password(value));
            Assert.Equal($"パスワードの文字数が不正です。文字数：{value.Length}", ex.Message);
        }


        [Theory]
        [InlineData("testmoney")]
        [InlineData("12345678")]
        [InlineData("1234@/%&")]
        public void パスワードの形式が不正の場合に例外が発生することを確認する(string value)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Password(value));
            Assert.Equal("パスワードの形式が不正です。", ex.Message);
        }
    }
}
