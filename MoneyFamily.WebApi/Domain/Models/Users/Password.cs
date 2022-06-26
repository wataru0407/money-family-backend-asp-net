using System.Text.RegularExpressions;

namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public record Password
    {
        public static readonly Password Empty = new Password(string.Empty);

        public const int MinPasswordLength = 8;
        public const int MaxPasswordLength = 32;
        public string Value { get; }

        public Password(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            if (value.Length < MinPasswordLength || value.Length > MaxPasswordLength) throw new ArgumentException($"パスワードの文字数が不正です。文字数：{value.Length}");
            if (!IsValidPassword(value)) throw new ArgumentException("パスワードの形式が不正です。");
            Value = value;
        }

        //半角英数字のみ、かつ英字1文字以上数字1文字以上使うルールとする
        private static bool IsValidPassword(string value)
        {
            // 半角英数字のみ
            if (Regex.IsMatch(value, @"^[0-9a-zA-Z]+$"))
            {
                // 半角英字のみ
                if (Regex.IsMatch(value, @"^[a-zA-Z]+$")) return false;
                // 半角数字のみ
                if (Regex.IsMatch(value, @"^[0-9]+$")) return false;
                return true;
            }
            return false;
        }
    }
}
