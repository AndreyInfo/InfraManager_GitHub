using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System.Linq;

namespace InfraManager.BLL.ProductCatalogue.Manufactures;

internal sealed class ManufacturerQueryBuilder :
    IBuildEntityQuery<Manufacturer, ManufacturerDetails, ManufacturersFilter>
    , ISelfRegisteredService<IBuildEntityQuery<Manufacturer, ManufacturerDetails, ManufacturersFilter>>
{
    private readonly IReadonlyRepository<Manufacturer> _repository;

    public ManufacturerQueryBuilder(IReadonlyRepository<Manufacturer> repository)
    {
        _repository = repository;
    }

    public IExecutableQuery<Manufacturer> Query(ManufacturersFilter filterBy)
    {
        var query =  _repository.Query();

        if (!string.IsNullOrWhiteSpace(filterBy.Name))
            query = query.Where(x => x.Name.Contains(filterBy.Name));

        if (filterBy.IsCable.HasValue) 
            query = query.Where(x => x.IsCable == filterBy.IsCable);
        
        if (filterBy.IsRack.HasValue) 
            query = query.Where(x => x.IsRack == filterBy.IsRack);
        
        if (filterBy.IsPanel.HasValue) 
            query = query.Where(x => x.IsPanel == filterBy.IsPanel);
        
        if (filterBy.IsNetworkDevice.HasValue) 
            query = query.Where(x => x.IsNetworkDevice == filterBy.IsNetworkDevice);
        
        if (filterBy.IsComputer.HasValue) 
            query = query.Where(x => x.IsComputer == filterBy.IsComputer);
        
        if (filterBy.IsOutlet.HasValue) 
            query = query.Where(x => x.IsOutlet == filterBy.IsOutlet);
        
        if (filterBy.IsCableCanal.HasValue) 
            query = query.Where(x => x.IsCableCanal == filterBy.IsCableCanal);
        
        if (filterBy.IsSoftware.HasValue) 
            query = query.Where(x => x.IsSoftware == filterBy.IsSoftware);
        
        if (filterBy.IsMaterials.HasValue) 
            query = query.Where(x => x.IsMaterials == filterBy.IsMaterials);
  
        return query;
    }
}