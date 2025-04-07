using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class NegotiationLoader : 
        ILoadEntity<Guid, Negotiation>,
        ISelfRegisteredService<ILoadEntity<Guid, Negotiation>>
    {
        IFindEntityByGlobalIdentifier<Negotiation> _finder;

        public NegotiationLoader(IFindEntityByGlobalIdentifier<Negotiation> finder)
        {
            _finder = finder;
        }

        public Task<Negotiation> LoadAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _finder
                .WithMany(x => x.NegotiationUsers)
                    .ThenWith(x => x.User)
                        .ThenWith(x => x.Subdivision)
                .WithMany(x => x.NegotiationUsers)
                    .ThenWith(x => x.User)
                        .ThenWith(x => x.Position)
                .FindOrRaiseErrorAsync(id, cancellationToken);
        }
    }
}
