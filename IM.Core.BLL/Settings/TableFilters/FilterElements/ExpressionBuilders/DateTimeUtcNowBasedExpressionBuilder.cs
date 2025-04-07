using InfraManager.Linq;
using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders
{
    public class DateTimeUtcNowBasedExpressionBuilder : IDateTimePredicateBuilder
    {
        private readonly Func<DateTime, DateTime> _getStartDate;
        private readonly Func<DateTime, DateTime> _getFinishDate;

        public DateTimeUtcNowBasedExpressionBuilder(
            Func<DateTime, DateTime> getStartDate, 
            Func<DateTime, DateTime> getFinishDate)
        {
            _getStartDate = getStartDate;
            _getFinishDate = getFinishDate;
        }

        public Expression<Func<DateTime, bool>> Build(DateTime? start, DateTime? finish, bool onlyDate)
        {
            var now = DateTime.UtcNow;
            return ExpressionExtensions.DateTimeBetween(_getStartDate(now), _getFinishDate(now), onlyDate);
        }

        public Expression<Func<DateTime?, bool>> BuildNullable(DateTime? start, DateTime? finish, bool onlyDate)
        {
            var now = DateTime.UtcNow;
            return ExpressionExtensions.NullableDateTimeBetween(_getStartDate(now), _getFinishDate(now), onlyDate);
        }
    }
}
