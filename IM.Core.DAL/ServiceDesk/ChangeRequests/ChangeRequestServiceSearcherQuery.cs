using InfraManager;
using InfraManager.DAL.Search;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestServiceSearcherQuery : 
        IChangeRequestServiceSearcherQuery,
        ISelfRegisteredService<IChangeRequestServiceSearcherQuery>
    {
        private readonly DbSet<Service> _services;

        public ChangeRequestServiceSearcherQuery(DbSet<Service> services)
        {
            _services = services;
        }

        public IQueryable<ObjectSearchResult> Query(string text)
        {
            IQueryable<Service> query = _services.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(text))
            {
                var searchPattern = $"%{text.ToStartsWithPattern()}";
                query = query
                    .Where(x => EF.Functions.Like(x.Name.ToLower(), searchPattern.ToLower()));
            }

            return query
                .Select(
                    x => new ObjectSearchResult
                    {
                        ID = x.ID,
                        ClassID = ObjectClass.Service,
                        FullName = x.Name
                    });
        }
    }
}
