using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.Search
{
    internal class CallTypeSearchQuery : ICallTypeSearchQuery, ISelfRegisteredService<ICallTypeSearchQuery>
    {
        private readonly DbSet<CallType> _callTypes;

        public CallTypeSearchQuery(DbSet<CallType> callTypes)
        {
            _callTypes = callTypes;
        }

        public IQueryable<ObjectSearchResult> Query(SearchCriteria searchCriteria)
        {
            IQueryable<CallType> query = _callTypes.AsNoTracking().Where(x => !x.Removed);

            if (!string.IsNullOrWhiteSpace(searchCriteria.Text))
            {
                var pattern = string.Concat("%", searchCriteria.Text.ToStartsWithPattern());
                query = query.Where(
                    x => EF.Functions.Like(CallType.GetFullCallTypeName(x.ID), pattern));
            }

            return query.Select(
                x => new ObjectSearchResult
                {
                    ID = x.ID,
                    FullName = CallType.GetFullCallTypeName(x.ID)
                });
        }
    }
}
