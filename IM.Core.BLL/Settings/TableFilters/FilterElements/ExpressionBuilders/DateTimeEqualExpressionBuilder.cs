using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders
{
    public class DateTimeEqualExpressionBuilder : IDateTimePredicateBuilder
    {
        public Expression<Func<DateTime, bool>> Build(DateTime? start, DateTime? finish, bool onlyDate)
        {
            if (!start.HasValue)
            {
                throw new ArgumentNullException(
                    "DateTimeEqualExpressionBuilder requires start date",
                    nameof(start));
            }

            var minDate = onlyDate ? start.Value.Date : start.Value.TruncateSeconds();
            var maxDate = onlyDate ? minDate.AddDays(1) : minDate.AddMinutes(1);

            return onlyDate
                ? dt => dt.Date == maxDate.Date
                : dt => dt >= minDate && dt < maxDate;
        }

        public Expression<Func<DateTime?, bool>> BuildNullable(DateTime? start, DateTime? finish, bool onlyDate)
        {
            if (!start.HasValue)
            {
                throw new ArgumentNullException(
                    "DateTimeEqualExpressionBuilder requires start date",
                    nameof(start));
            }

            var minDate = onlyDate ? start.Value.Date : start.Value.TruncateSeconds();
            var maxDate = onlyDate ? minDate.AddDays(1) : minDate.AddMinutes(1);

            return onlyDate
                ? dt => dt.HasValue && dt.Value.Date == maxDate.Date
                : dt => dt >= minDate && dt < maxDate;
        }
    }
}
