using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Manhours;

namespace InfraManager.BLL.ServiceDesk.Manhours
{
    internal class ManhoursWorkLoader : ILoadEntity<Guid, ManhoursWork>, ISelfRegisteredService<ILoadEntity<Guid, ManhoursWork>>
    {
        readonly IFindEntityByGlobalIdentifier<ManhoursWork> _finder;

        public ManhoursWorkLoader(IFindEntityByGlobalIdentifier<ManhoursWork> finder)
        {
            _finder = finder;
        }

        public Task<ManhoursWork> LoadAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _finder
                .WithMany(x => x.Entries)
                .FindOrRaiseErrorAsync(id, cancellationToken);
        }
    }
}
