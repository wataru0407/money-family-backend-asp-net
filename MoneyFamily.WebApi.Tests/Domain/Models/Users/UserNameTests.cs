using MoneyFamily.WebApi.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFamily.WebApi.Tests.Domain.Models.Users
{
    public class UserNameTests
    {
        [Theory]
        [InlineData("test")]
        [InlineData("123")]
        [InlineData("test123")]
        [InlineData("てすと")]
        [InlineData("テスト")]
        [InlineData("試験")]
        public void 正常にインスタンスが生成できることを確認する(string value)
        {
            var expected = value;
            var userName = new UserName(expected);
            var actual = userName.Value;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void 値がnullや空白の場合に例外が発生することを確認する(string value)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new UserName(value));
            Assert.Equal("Value cannot be null. (Parameter 'value')", ex.Message);
        }

        [Theory]
        [InlineData("あ")]
        [InlineData("あいうえおかきくけこさ")]
        public void 文字数が不正の場合に例外が発生することを確認する(string value)
        {
            var ex = Assert.Throws<ArgumentException>(() => new UserName(value));
            Assert.Equal($"ユーザ名の文字数が不正です。文字数：{value.Length}", ex.Message);
        }

        [Theory]
        [InlineData("[]@q")]
        [InlineData("./&d1")]
        [InlineData("@abc&)")]
        //[InlineData("1w$./")]
        //[InlineData("w$./")]
        public void 名前に使用できない文字がある場合に例外が発生することを確認する(string value)
        {
            var ex = Assert.Throws<ArgumentException>(() => new UserName(value));
            Assert.Equal("ユーザ名に使用できない文字があります。", ex.Message);
        }
    }
}
