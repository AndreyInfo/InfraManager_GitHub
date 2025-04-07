using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    internal static class PriorityRepositoryExtensions
    {
        public static async Task<Guid?> GetDefaultPriorityIDAsync<T>(
            this IReadonlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IDefault
        {
            var defaultPriority = await repository.GetDefaultAsync(cancellationToken);
            return defaultPriority?.ID;
        }

        public static Task<T> GetDefaultAsync<T>(
            this IReadonlyRepository<T> repository,
            CancellationToken cancellationToken = default)
            where T : class, IDefault
        {
            return repository.FirstOrDefaultAsync(x => x.Default, cancellationToken);
        }
    }
}
