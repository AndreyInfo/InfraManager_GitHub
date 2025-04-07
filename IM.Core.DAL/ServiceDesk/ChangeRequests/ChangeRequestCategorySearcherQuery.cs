using InfraManager;
using InfraManager.DAL.Search;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestCategorySearcherQuery : 
        IChangeRequestCategorySearcherQuery,
        ISelfRegisteredService<IChangeRequestCategorySearcherQuery>
    {
        private readonly DbSet<ChangeRequestCategory>  _changeRequestCategories;

        public ChangeRequestCategorySearcherQuery(DbSet<ChangeRequestCategory> changeRequestCategories)
        {
            _changeRequestCategories = changeRequestCategories;
        }

        public IQueryable<ObjectSearchResult> Query(string text)
        {
            IQueryable<ChangeRequestCategory> query = _changeRequestCategories.AsNoTracking();

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
                        ClassID = ObjectClass.ChangeRequestCategory,
                        FullName = x.Name
                    });
        }
    }
}
