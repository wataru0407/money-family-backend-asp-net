using MoneyFamily.WebApi.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFamily.WebApi.Tests.Domain.Models.Users
{
    public class UserIdTests
    {
        [Fact]
        public void 正常にインスタンスが生成できることを確認する()
        {
            var expected = Guid.NewGuid();
            var userId = new UserId(expected);
            var actual = userId.Value;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Guidがnullの場合に例外が発生することを確認する()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new UserId(Guid.Empty));
            Assert.Equal("Value cannot be null. (Parameter 'value')", ex.Message);
        }
    }
}
