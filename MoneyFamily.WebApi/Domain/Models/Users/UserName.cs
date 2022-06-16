using System.Text.RegularExpressions;

namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public record UserName
    {
        public const int MinUserNameLength = 2;
        public const int MaxUserNameLength = 10;
        public string Value { get; }

        public UserName(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            if (value.Length < MinUserNameLength || value.Length > MaxUserNameLength) throw new ArgumentException($"ユーザ名の文字数が不正です。文字数：{value.Length}");
            if (!Regex.IsMatch(value, @"^[\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}0-9a-zA-Z]+"))
            {
                throw new ArgumentException($"ユーザ名に使用できない文字があります。");
            }
            Value = value;
        }
    }
}
