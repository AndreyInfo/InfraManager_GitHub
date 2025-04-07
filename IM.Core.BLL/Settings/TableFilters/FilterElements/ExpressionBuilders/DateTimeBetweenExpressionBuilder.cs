using InfraManager.Linq;
using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders
{
    public class DateTimeBetweenExpressionBuilder : IDateTimePredicateBuilder
    {
        public Expression<Func<DateTime, bool>> Build(DateTime? start, DateTime? finish, bool onlyDate)
        {
            if (!start.HasValue)
            {
                throw new ArgumentNullException(
                    "DateTimeBetweenExpressionBuilder requires start date",
                    nameof(start));
            }

            if (!finish.HasValue)
            {
                throw new ArgumentNullException(
                    "DateTimeBetweenExpressionBuilder requires finish date",
                    nameof(finish));
            }

            return ExpressionExtensions.DateTimeBetween(start, finish, onlyDate);
        }

        public Expression<Func<DateTime?, bool>> BuildNullable(DateTime? start, DateTime? finish, bool onlyDate)
        {
            if (!start.HasValue)
            {
                throw new ArgumentNullException(
                    "DateTimeBetweenExpressionBuilder requires start date",
                    nameof(start));
            }

            if (!finish.HasValue)
            {
                throw new ArgumentNullException(
                    "DateTimeBetweenExpressionBuilder requires finish date",
                    nameof(finish));
            }

            return ExpressionExtensions.NullableDateTimeBetween(start, finish, onlyDate);
        }
    }
}
