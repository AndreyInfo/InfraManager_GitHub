using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.OrganizationStructure
{
    internal class SubdivisionFullNameQuery : 
        ISubdivisionFullNameQuery, 
        ISelfRegisteredService<ISubdivisionFullNameQuery>
    {
        private readonly DbSet<Subdivision> _db;

        public SubdivisionFullNameQuery(DbSet<Subdivision> db)
        {
            _db = db;
        }
//+
        public Task<string> QueryAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // TODO: Add CTE support to perform queries like this without stored functions
            return _db
                .Where(x => x.ID == id)
                .Select(x => Subdivision.GetFullSubdivisionName(x.ID))
                .SingleAsync();
        }
    }
}
