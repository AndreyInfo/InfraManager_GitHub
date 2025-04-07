using InfraManager;
using InfraManager.DAL.Search;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestTypeSearcherQuery : 
        IChangeRequestTypeSearcherQuery,
        ISelfRegisteredService<IChangeRequestTypeSearcherQuery>
    {
        private readonly DbSet<ChangeRequestType> _changeRequestTypes;

        public ChangeRequestTypeSearcherQuery(DbSet<ChangeRequestType> changeRequestTypes)
        {
            _changeRequestTypes = changeRequestTypes;
        }

        public IQueryable<ObjectSearchResult> Query(string text)
        {
            IQueryable<ChangeRequestType> query = _changeRequestTypes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(text))
            {
                var searchPattern = $"%{text.ToStartsWithPattern()}";
                query = query
                    .Where(x => !x.Removed)
                    .Where(x => EF.Functions.Like(x.Name.ToLower(), searchPattern.ToLower()));
            }

            return query
                .Select(
                    x => new ObjectSearchResult
                    {
                        ID = x.ID,
                        ClassID = ObjectClass.ChangeRequestType,
                        FullName = x.Name
                    });
        }
    }
}
