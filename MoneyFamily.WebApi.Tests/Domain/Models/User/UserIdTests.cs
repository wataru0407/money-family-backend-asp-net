using MoneyFamily.WebApi.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFamily.WebApi.Tests.Domain.Models.User
{
    public class UserIdTests
    {
        [Fact]
        public void 正常にインスタンスを生成するテスト()
        {
            var expected = Guid.NewGuid();
            var userId = new UserId(expected);
            var actual = userId.Value;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Guidがnullの場合に例外が発生するテスト()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new UserId(Guid.Empty));
            Assert.Equal("Value cannot be null. (Parameter 'value')", ex.Message);
        }
    }
}
