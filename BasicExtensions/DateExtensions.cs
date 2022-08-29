using System;
using System.Collections.Generic;

namespace BasicExtensions
{
    public static class DateExtensions
    {
        public static IEnumerable<DateTime> EachDay(this DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }
        public static string ToDatetimeString(this DateTime date, string format = Models.DatetimeFormat.ShortDateFormat.DbFormat)
        {
            var response = string.Empty;
            switch (format)
            {
                case Models.DatetimeFormat.ShortDateFormat.TrFormat1:
                    response = $"{date.Day.PadLeft()}.{date.Month.PadLeft()}.{date.Year}";
                    break;
                case Models.DatetimeFormat.ShortDateFormat.TrFormat2:
                    response = $"{date.Day.PadLeft()}/{date.Month.PadLeft()}/{date.Year}";
                    break;
                case Models.DatetimeFormat.LongDateTimeFormat.TrFormat1:
                    response = $"{date.Day.PadLeft()}.{date.Month.PadLeft()}.{date.Year} {date.Hour.PadLeft()}:{date.Minute.PadLeft()}:{date.Second.PadLeft()}";
                    break;
                case Models.DatetimeFormat.LongDateTimeFormat.TrFormat2:
                    response = $"{date.Day.PadLeft()}/{date.Month.PadLeft()}/{date.Year} {date.Hour.PadLeft()}:{date.Minute.PadLeft()}:{date.Second.PadLeft()}";
                    break;
                case Models.DatetimeFormat.ShortDateFormat.DbFormat:
                    response = $"{date.Year}-{date.Month.PadLeft()}-{date.Day.PadLeft()}";
                    break;
                case Models.DatetimeFormat.LongDateTimeFormat.DbFormat:
                    response = $"{date.Year}-{date.Month.PadLeft()}-{date.Day.PadLeft()} {date.Hour.PadLeft()}:{date.Minute.PadLeft()}:{date.Second.PadLeft()}";
                    break;
                case Models.DatetimeFormat.TimeFormat.Format1:
                    response = $"{date.Hour.PadLeft()}:{date.Minute.PadLeft()}";
                    break;
                case Models.DatetimeFormat.TimeFormat.Format2:
                    response = $"{date.Hour.PadLeft()}:{date.Minute.PadLeft()}:{date.Second.PadLeft()}";
                    break;
                case Models.DatetimeFormat.TimeFormat.Format3:
                    response = $"{date.Hour.PadLeft()}:{date.Minute.PadLeft()}:{date.Second.PadLeft()}.{date.Millisecond.ToString().ToCustomSubstring(3).PadRight('0',3)}";
                    break;
                default:
                    response = $"{date.Year}-{date.Month.PadLeft()}-{date.Day.PadLeft()}";
                    break;
            }
            return response;
        }
        public static DateTime ToStartDatetime(this DateTime date) => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        public static DateTime ToEndDatetime(this DateTime date) => new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        public static string ToStartDatetimeString(this DateTime date, string format = Models.DatetimeFormat.ShortDateFormat.DbFormat) => date.ToStartDatetime().ToDatetimeString(format);
        public static string ToEndDatetimeString(this DateTime date, string format = Models.DatetimeFormat.ShortDateFormat.DbFormat) => date.ToEndDatetime().ToDatetimeString(format);
        public static DateTime ToDatetime(this string date) => DateTime.TryParse(date,out DateTime response) ? response : DateTime.MinValue;
        public static DateTime ToDatetime(this string date,IFormatProvider provider) => DateTime.TryParse(date,provider,System.Globalization.DateTimeStyles.None, out DateTime response) ? response : DateTime.MinValue;
        public static DateTime ToDatetime(this string date, IFormatProvider provider,System.Globalization.DateTimeStyles styles) => DateTime.TryParse(date, provider, styles, out DateTime response) ? response : DateTime.MinValue;
        public static DateTime? ToDatetimeNullable(this string date) => DateTime.TryParse(date, out DateTime response) ? response : (DateTime?)null;
        public static DateTime? ToDatetimeNullable(this string date, IFormatProvider provider) => DateTime.TryParse(date, provider, System.Globalization.DateTimeStyles.None, out DateTime response) ? response : (DateTime?)null;
        public static DateTime? ToDatetimeNullable(this string date, IFormatProvider provider, System.Globalization.DateTimeStyles styles) => DateTime.TryParse(date, provider, styles, out DateTime response) ? response : (DateTime?)null;
        public static DateTime ToFirstDateOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);
        public static DateTime ToLastDateOfMonth(this DateTime date) => new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        public static bool IsWeekend(this DateTime value) => value.DayOfWeek == DayOfWeek.Saturday || value.DayOfWeek == DayOfWeek.Sunday;
    }
}
