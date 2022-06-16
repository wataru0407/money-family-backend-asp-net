using MoneyFamily.WebApi.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFamily.WebApi.Tests.Domain.Models.User
{
    public class UserNameTests
    {
        [Theory]
        [InlineData("test123")]
        [InlineData("てすと")]
        [InlineData("テスト")]
        [InlineData("試験")]
        public void 正常にインスタンスを生成するテスト(string name)
        {
            var expected = name;
            var userName = new UserName(expected);
            var actual = userName.Value;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void 値がnullや空白の場合に例外が発生するテスト(string name)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new UserName(name));
            Assert.Equal("Value cannot be null. (Parameter 'value')", ex.Message);
        }

        [Theory]
        [InlineData("あ")]
        [InlineData("あいうえおかきくけこさ")]
        //[InlineData("1w$")]
        public void 指定文字数の上限下限を超えると例外が発生するテスト(string name)
        {
            Action actual = () => new UserName(name);
            var length = name.Length;
            var ex = Assert.Throws<ArgumentException>(actual);

            //var ex = Assert.Throws<ArgumentNullException>(() => new UserName(name));
            Assert.Equal($"ユーザ名の文字数が不正です。文字数：{name.Length}", ex.Message);
        }

        [Theory]
        [InlineData("[]@")]
        [InlineData("./&")]
        //[InlineData("1w$")]
        public void 名前に使用できない文字がある場合に例外が発生するテスト(string name)
        {
            Action actual = () => new UserName(name);
            var ex = Assert.Throws<ArgumentException>(actual);

            //var ex = Assert.Throws<ArgumentNullException>(() => new UserName(name));
            Assert.Equal("ユーザ名に使用できない文字があります。", ex.Message);
        }
    }
}
