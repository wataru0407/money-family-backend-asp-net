using System.Globalization;

namespace MoneyFamily.WebApi.Domain.Models.VariableCost
{
    public record VariableCostDate
    {
        // 日付
        public DateTime DateTime { get; }
        // 年
        public int Year { get; }
        // 月
        public int Month { get; }
        // 日にち
        public int Day { get; }
        // 曜日
        public int DayOfWeek { get; }
        // 週番号
        public int WeekOfYear { get; }

        public VariableCostDate(DateTime dateTime)
        {
            DateTime = dateTime;
            Year = dateTime.Year;
            Month = dateTime.Month;
            Day = dateTime.Day;
            
            //月曜を0、日曜を6とする
            DayOfWeek = ((int)dateTime.DayOfWeek + 6) % 7;

            // 月曜始まりで1年のなかで何番目の週かを設定する
            WeekOfYear = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFullWeek, System.DayOfWeek.Monday);
        }

    }
}
