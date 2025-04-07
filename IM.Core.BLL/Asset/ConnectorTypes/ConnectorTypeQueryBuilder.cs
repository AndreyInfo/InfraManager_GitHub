using Inframanager.BLL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.Asset.ConnectorTypes;
internal sealed class ConnectorTypeQueryBuilder : IBuildEntityQuery<ConnectorType, ConnectorTypeDetails, BaseFilter>
    , ISelfRegisteredService<IBuildEntityQuery<ConnectorType, ConnectorTypeDetails, BaseFilter>>
{
    private readonly IReadonlyRepository<ConnectorType> _repository;

    public ConnectorTypeQueryBuilder(IReadonlyRepository<ConnectorType> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<ConnectorType> Query(BaseFilter filterBy)
    {
        return _repository.With(x => x.Medium).Query();
    }
}
