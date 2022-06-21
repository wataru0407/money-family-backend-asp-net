using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public record EmailAddress
    {
        public string Value { get; }

        public EmailAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            if (!IsValidEmail(value)) throw new ArgumentException("メールアドレスの形式が不正です。");

            Value = value;
        }

        private static bool IsValidEmail(string value)
        {
            try
            {
                var email = new MailAddress(value);
                return email.Address == value;
            }
            catch
            {
                return false;
            }
        }
    }
}
