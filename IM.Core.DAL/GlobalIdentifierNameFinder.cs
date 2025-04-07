using Microsoft.EntityFrameworkCore;
using RTools_NTS.Util;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    internal class GlobalIdentifierNameFinder<TEntity> : IFindNameByGlobalID 
        where TEntity : class, INamedEntity, IGloballyIdentifiedEntity
    {
        private readonly DbSet<TEntity> _db;

        public GlobalIdentifierNameFinder(DbSet<TEntity> db)
        {
            _db = db;
        }

        public string Find(Guid id)
        {
            var entity = _db.SingleOrDefault(x => x.IMObjID == id);

            return NameFinder<TEntity>.GetNameOrDefault(entity);
        }

        public async Task<string> FindAsync(Guid id, CancellationToken token = default)
        {
            var entity = await _db.SingleOrDefaultAsync(x => x.IMObjID == id, token);

            return NameFinder<TEntity>.GetNameOrDefault(entity);
        }
    }
}
