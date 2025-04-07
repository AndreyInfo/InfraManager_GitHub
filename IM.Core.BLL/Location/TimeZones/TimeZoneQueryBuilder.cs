using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Location.TimeZones;

internal class TimeZoneQueryBuilder: 
    IBuildEntityQuery<TimeZone, TimeZoneDetails, TimeZoneListFilter>,
    ISelfRegisteredService<IBuildEntityQuery<TimeZone, TimeZoneDetails, TimeZoneListFilter>>
{
    private readonly IReadonlyRepository<TimeZone> _repository;

    public TimeZoneQueryBuilder(IReadonlyRepository<TimeZone> repository) => _repository = repository;
    
    public IExecutableQuery<TimeZone> Query(TimeZoneListFilter filterBy)
    {
        var query = _repository.WithMany(x => x.TimeZoneAdjustmentRules).DisableTrackingForQuery().Query();
        
        if (!string.IsNullOrWhiteSpace(filterBy.Name))
        {
            query = query.Where(timeZone => timeZone.Name.ToLower().Contains(filterBy.Name.ToLower()));
        }

        return query;
    }
}