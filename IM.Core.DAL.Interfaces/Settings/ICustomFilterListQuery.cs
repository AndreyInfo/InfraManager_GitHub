using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.Settings
{
    public interface ICustomFilterListQuery
    {
        IQueryable<T> Query<T> (IEnumerable<FilterElementData> filter) where T: class;
    }
}
