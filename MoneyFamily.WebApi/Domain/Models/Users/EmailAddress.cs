using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public record EmailAddress
    {
        public string Value { get; }

        public EmailAddress(string value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));

            var attr = new EmailAddressAttribute();
            if (!attr.IsValid(value)) throw new ArgumentException("メールアドレスの形式が不正です。");

            Value = value;
        }
    }
}
