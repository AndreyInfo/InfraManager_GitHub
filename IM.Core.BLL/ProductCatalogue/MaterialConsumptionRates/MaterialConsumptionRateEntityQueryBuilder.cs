using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.MaterialConsumptionRates;

public class MaterialConsumptionRateEntityQuery:
    IBuildEntityQuery<MaterialConsumptionRate, MaterialConsumptionRateOutputDetails, MaterialConsumptionRateFilter>,
    ISelfRegisteredService< IBuildEntityQuery<MaterialConsumptionRate, MaterialConsumptionRateOutputDetails, MaterialConsumptionRateFilter>>
{
    private readonly IReadonlyRepository<MaterialConsumptionRate> _materialConsumptionRates;

    public MaterialConsumptionRateEntityQuery(IReadonlyRepository<MaterialConsumptionRate> materialConsumptionRates)
    {
        _materialConsumptionRates = materialConsumptionRates;
    }

    public IExecutableQuery<MaterialConsumptionRate> Query(MaterialConsumptionRateFilter filterBy)
    {
        var query = _materialConsumptionRates.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.DeviceModelID))
            query = query.Where(x => x.DeviceModelID.Contains(filterBy.DeviceModelID));
       
        if (!string.IsNullOrWhiteSpace(filterBy.MaterialModelName))
            query = query.Where(x => x.Model.Name.Contains(filterBy.MaterialModelName));
        
        if (filterBy.HasUseBWPrint)
            query = query.Where(x => x.UseBWPrint == filterBy.UseBWPrint);
        
        if (filterBy.HasUseColorPrint)
            query = query.Where(x => x.UseColorPrint == filterBy.UseColorPrint);
        
        if (filterBy.HasUsePhotoPrint)
            query = query.Where(x => x.UsePhotoPrint == filterBy.UsePhotoPrint);

        return query;
    }
}