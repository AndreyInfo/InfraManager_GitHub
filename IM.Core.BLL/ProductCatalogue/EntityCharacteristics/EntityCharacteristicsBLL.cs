using InfraManager.DAL.ProductCatalogue;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
internal class EntityCharacteristicsBLL : IEntityCharacteristicsBLL, ISelfRegisteredService<IEntityCharacteristicsBLL>
{
    private readonly IServiceMapper<ProductTemplate, IEntityCharacteristicsProvider> _serviceMapper;

    public EntityCharacteristicsBLL(IServiceMapper<ProductTemplate, IEntityCharacteristicsProvider> serviceMapper)
    {
        _serviceMapper = serviceMapper;
    }

    public Task<EntityCharacteristicsDetailsBase> GetAsync(Guid id, ProductTemplate templateID, CancellationToken cancellationToken)
        => _serviceMapper.Map(templateID).GetAsync(id, cancellationToken);

    public Task<EntityCharacteristicsDetailsBase> UpdateAsync(Guid id, ProductTemplate templateID, EntityCharacteristicsDataBase data, CancellationToken cancellationToken)
        => _serviceMapper.Map(templateID).UpdateAsync(id, data, cancellationToken);
}
