using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders
{
    public class DateTimeAfterExpressionBuilder : IDateTimePredicateBuilder
    {
        public Expression<Func<DateTime, bool>> Build(DateTime? start, DateTime? finish, bool onlyDate)
        {
            if (!start.HasValue)
            {
                throw new ArgumentNullException(
                    "DateTimeAfterExpressionBuilder requires start date", 
                    nameof(start));
            }

            var date = onlyDate 
                ? start.Value.Date.AddDays(1) 
                : start.Value.TruncateSeconds().AddMinutes(1);

            return onlyDate
                ? dt => dt.Date > date.Date
                : dt => dt >= date;
        }

        public Expression<Func<DateTime?, bool>> BuildNullable(DateTime? start, DateTime? finish, bool onlyDate)
        {
            if (!start.HasValue)
            {
                throw new ArgumentNullException(
                    "DateTimeAfterExpressionBuilder requires start date",
                    nameof(start));
            }

            var date = onlyDate
                ? start.Value.Date.AddDays(1)
                : start.Value.TruncateSeconds().AddMinutes(1);

            return onlyDate
                ? dt => dt.HasValue && dt.Value.Date > date.Date
                : dt => dt >= date;
        }
    }
}
