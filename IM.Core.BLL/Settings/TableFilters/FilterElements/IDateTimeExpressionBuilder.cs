using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    public interface IDateTimePredicateBuilder
    {
        Expression<Func<DateTime, bool>> Build(DateTime? start, DateTime? finish, bool onlyDate);
        Expression<Func<DateTime?, bool>> BuildNullable(DateTime? start, DateTime? finish, bool onlyDate);
        // TODO: Second method needds to be refactored
    }
}
