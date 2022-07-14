using System.Globalization;

namespace MoneyFamily.WebApi.Domain.Models.VariableCost
{
    public record VariableCostDate
    {
        public DateTime DateTime { get; }
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public int DayOfWeek { get; }
        public int WeekOfYear { get; }

        public VariableCostDate(DateTime dateTime)
        {
            DateTime = dateTime;
            Year = dateTime.Year;
            Month = dateTime.Month;
            Day = dateTime.Day;
            DayOfWeek = (int)dateTime.DayOfWeek;
        }

    }
}
