using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    internal class ChangeRequestCategoryLookupQuery : ILookupQuery
    {
        private readonly DbSet<ChangeRequest> _changeRequests;
        private readonly DbSet<ChangeRequestCategory> _changeRequestCategories;

        public ChangeRequestCategoryLookupQuery(DbSet<ChangeRequest> changeRequests, DbSet<ChangeRequestCategory> changeRequestCategories)
        {
            _changeRequests = changeRequests;
            _changeRequestCategories = changeRequestCategories;
        }

        public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var query = from changeRequest in _changeRequests.AsNoTracking()
                        join changeRequestCategory in _changeRequestCategories.AsNoTracking()
                        on changeRequest.CategoryID equals changeRequestCategory.ID
                        select new
                        {
                            changeRequestCategory.ID,
                            Info = changeRequestCategory.Name
                        };

            return Array.ConvertAll(await query.Distinct().ToArrayAsync(cancellationToken), x => new ValueData { ID = x.ID.ToString(), Info = x.Info });
        }
    }
}
