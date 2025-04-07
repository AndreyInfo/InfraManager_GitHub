using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.ConnectorTypes;
internal sealed class ConnectorTypeLoader : ILoadEntity<int, ConnectorType>
    , ISelfRegisteredService<ILoadEntity<int, ConnectorType>>
{
    private readonly IReadonlyRepository<ConnectorType> _repository;

    public ConnectorTypeLoader(IReadonlyRepository<ConnectorType> repository)
    {
        _repository = repository;
    }

    public Task<ConnectorType> LoadAsync(int id, CancellationToken cancellationToken = default)
    {
        return _repository
            .With(x => x.Medium)
            .FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
    }
}
