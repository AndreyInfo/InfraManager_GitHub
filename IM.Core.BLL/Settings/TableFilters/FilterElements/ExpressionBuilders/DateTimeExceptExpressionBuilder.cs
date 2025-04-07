using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders
{
    public class DateTimeExceptExpressionBuilder : IDateTimePredicateBuilder
    {
        public Expression<Func<DateTime, bool>> Build(DateTime? start, DateTime? finish, bool onlyDate)
        {
            if (!start.HasValue)
            {
                throw new ArgumentNullException(
                    "DateTimeExceptExpressionBuilder requires start date",
                    nameof(start));
            }

            if (!finish.HasValue)
            {
                throw new ArgumentNullException(
                    "DateTimeBetweenExpressionBuilder requires finish date",
                    nameof(finish));
            }

            var minDate = onlyDate ? start.Value.Date : start.Value.TruncateSeconds();
            var maxDate = onlyDate ? finish.Value.Date.AddDays(1) : finish.Value.TruncateSeconds().AddSeconds(1);

            return onlyDate
                ? dt => dt.Date <= minDate.Date || dt.Date > maxDate.Date
                : dt => dt <= minDate || dt > maxDate;
        }

        public Expression<Func<DateTime?, bool>> BuildNullable(DateTime? start, DateTime? finish, bool onlyDate)
        {
            if (!start.HasValue)
            {
                throw new ArgumentNullException(
                    "DateTimeExceptExpressionBuilder requires start date",
                    nameof(start));
            }

            if (!finish.HasValue)
            {
                throw new ArgumentNullException(
                    "DateTimeBetweenExpressionBuilder requires finish date",
                    nameof(finish));
            }

            var minDate = onlyDate ? start.Value.Date : start.Value.TruncateSeconds();
            var maxDate = onlyDate ? finish.Value.Date.AddDays(1) : finish.Value.TruncateSeconds().AddSeconds(1);

            return onlyDate
                ? dt => !dt.HasValue || dt.Value.Date <= minDate.Date || dt.Value.Date > maxDate.Date
                : dt => dt <= minDate || dt > maxDate || !dt.HasValue;
        }
    }
}
