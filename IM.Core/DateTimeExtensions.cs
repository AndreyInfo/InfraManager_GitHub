using System;

namespace InfraManager
{
    public static class DateTimeExtensions
    {
        public static DateTime TruncateSeconds(this DateTime date)
        {
            return date.TruncateMilliseconds().AddSeconds(-date.Second);
        }

        public static DateTime TruncateMilliseconds(this DateTime date)
        {
            return date.AddMilliseconds(-date.Millisecond);
        }

        public static DateTime TruncateMinutes(this DateTime date)
        {
            return date.TruncateSeconds().AddMinutes(-date.Minute);
        }

        private const int MonthsInYear = 12;

        public static DateTime BeginningOfHalfYear(this DateTime date)
        {
            return new DateTime(date.Year, date.Month > MonthsInYear / 2 ? MonthsInYear / 2 : 1, 1);
        }

        public static DateTime AddHalfYear(this DateTime date)
        {
            return date.AddMonths(MonthsInYear / 2);
        }

        public static DateTime BeginningOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        private const int MonthsPerQuarter = 3;

        public static DateTime BeginningOfQuarter(this DateTime date)
        {
            return new DateTime(date.Year, (date.Month / MonthsPerQuarter) * MonthsPerQuarter, 1);
        }

        public static DateTime AddQuarter(this DateTime date, int quantity)
        {
            return date.AddMonths(MonthsPerQuarter * quantity);
        }

        public static DateTime BeginningOfWeek(this DateTime date)
        {
            return date.Date.AddDays(-(int)date.DayOfWeek); // TODO: This should depend on locale (for ru-ru week starts in monday)
        }

        public static bool TryConvertFromMillisecondsAfterMinimumDate(
            string value, out DateTime dateTime)
        {
            if (long.TryParse(value, out var milliseconds))
            {
                dateTime = MinimumDate.AddMilliseconds(milliseconds);
                return true;
            }

            dateTime = default;
            return false;
        }

        public static DateTime ConvertFromMillisecondsAfterMinimumDate(this string value)
        {
            return TryConvertFromMillisecondsAfterMinimumDate(value, out DateTime tmp)
                ? tmp
                : throw new Exception($"Invalid DateTime");
        }

        public static bool TryConvertFromMillisecondsAfterMinimumDate(
            string value, out DateTime? dateTime)
        {
            dateTime = null;

            if (TryConvertFromMillisecondsAfterMinimumDate(value, out DateTime convertedDateTime))
            {
                dateTime = convertedDateTime;
                return true;
            }

            return string.IsNullOrEmpty(value);
        }

        public static DateTime? NullableConvertFromMillisecondsAfterMinimumDate(
            this string value)
        {
            if (value == null)
            {
                return null;
            }

            return TryConvertFromMillisecondsAfterMinimumDate(value, out DateTime? tmp)
                ? tmp
                : throw new Exception($"Invalid DateTime");
        }

        public static string ConvertToMillisecondsAfterMinimumDate(this DateTime dateTime)
        {
            return ((long)dateTime.Subtract(MinimumDate).TotalMilliseconds).ToString();
        }

        public static string ConvertToMillisecondsAfterMinimumDate(this DateTime? dateTime)
        {
            return dateTime.HasValue
                ? dateTime.Value.ConvertToMillisecondsAfterMinimumDate()
                : string.Empty;
        }

        public static string Format(this DateTime? dateTime, string format)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(format) : string.Empty;
        }

        public static readonly DateTime MinimumDate = 
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Заменяет время у даты
        /// </summary>
        /// <param name="date">Дата, время которой нужно заменить</param>
        /// <param name="time">Время, на которое нужно заменить</param>
        public static DateTime ChangeTime(this DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Milliseconds);
        }

        /// <summary>
        /// Привести это время к времени с указанным смещением.
        /// </summary>
        /// <param name="utcDateTime">Время UTC.</param>
        /// <param name="utcOffsetInMinutes">Смешение в минутах относительно UTC.</param>
        /// <returns>Экземпляр <see cref="DateTimeOffset"/> с заданным смещением относительно UTC.</returns>
        public static DateTimeOffset ToOffset(this DateTime utcDateTime, int utcOffsetInMinutes)
        {
            return new DateTimeOffset(utcDateTime.Ticks, TimeSpan.Zero).ToOffset(TimeSpan.FromMinutes(utcOffsetInMinutes));
        }
    }
}
