using Inframanager.BLL;
using InfraManager.DAL;
using System.Threading;
using System.Threading.Tasks;
using ActivePortEntity = InfraManager.DAL.Asset.ActivePort;

namespace InfraManager.BLL.Asset.ActivePort;

internal sealed class ActivePortLoader : ILoadEntity<int, ActivePortEntity, ActivePortDetails>
    , ISelfRegisteredService<ILoadEntity<int, ActivePortEntity, ActivePortDetails>>
{
    private readonly IFinder<ActivePortEntity> _finder;

    public ActivePortLoader(IFinder<ActivePortEntity> finder)
    {
        _finder = finder;
    }

    public Task<ActivePortEntity> LoadAsync(int id, CancellationToken cancellationToken)
    {
        return _finder
            .With(x => x.JackType)
            .With(x => x.TechnologyType)
            .FindOrRaiseErrorAsync(id, cancellationToken);
    }
}
