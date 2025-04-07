using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    internal class AnyQuery<T> : IAnyQuery where T : class, IGloballyIdentifiedEntity
    {
        private readonly DbSet<T> _db;

        public AnyQuery(DbSet<T> db)
        {
            _db = db;
        }

        public Task<bool> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _db.AnyAsync(x => x.IMObjID == id, cancellationToken);
        }
    }
}
