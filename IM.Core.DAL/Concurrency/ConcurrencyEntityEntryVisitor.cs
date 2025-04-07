using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Concurrency
{
    /// <summary>
    /// Этот класс обеспечивает корректную работу оптимистических блокировок на RowVersion
    /// При смене RowVersion значение необходимо записать в OriginalState
    /// </summary>
    internal class ConcurrencyEntityEntryVisitor : IVisitEntityEntry, ISelfRegisteredService<IVisitEntityEntry>
    {
        public Task AfterSaveAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Visit(EntityEntry entry)
        {
            if (entry.State == EntityState.Modified)
            {
                foreach(var concurrencyToken 
                        in entry.Properties.Where(p => p.Metadata.IsConcurrencyToken && p.IsModified))
                {
                    concurrencyToken.OriginalValue = concurrencyToken.CurrentValue;
                }
            }
        }

        public void AfterSave()
        {
        }

        public Task VisitAsync(EntityEntry entry, CancellationToken cancellationToken)
        {
            Visit(entry);
            return Task.CompletedTask;
        }
    }
}
