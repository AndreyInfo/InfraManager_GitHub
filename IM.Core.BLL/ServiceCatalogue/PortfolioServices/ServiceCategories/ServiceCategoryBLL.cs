using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using InfraManager.Linq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalog;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;
using InfraManager;
using System.Linq.Expressions;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceCategories;

internal class ServiceCategoryBLL : IServiceCategoryBLL, ISelfRegisteredService<IServiceCategoryBLL>
{
    private readonly IRepository<ServiceCategory> _repository;
    private readonly IRepository<Service> _services;
    private readonly IRepository<ServiceItem> _serviceItems;
    private readonly IRepository<ServiceAttendance> _serviceAttendances;
    private readonly IRepository<CustomControl> _customControls;
    private readonly IRepository<SLAReference> _slaReferences;
    private readonly IFindEntityByGlobalIdentifier<User> _userFinder;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly ISLAQuery _slaQuery;

    public ServiceCategoryBLL(
        IRepository<ServiceCategory> repository,
        IRepository<Service> services,
        IRepository<ServiceItem> serviceItems,
        IRepository<ServiceAttendance> serviceAttendances,
        IRepository<CustomControl> customControls,
        IRepository<SLAReference> slaReferences,
        IFindEntityByGlobalIdentifier<User> userFinder,
        IMapper mapper,
        ICurrentUser currentUser,
        ISLAQuery slaQuery)
    {
        _repository = repository;
        _services = services;
        _serviceItems = serviceItems;
        _serviceAttendances = serviceAttendances;
        _customControls = customControls;
        _slaReferences = slaReferences;
        _userFinder = userFinder;
        _mapper = mapper;
        _currentUser = currentUser;
        _slaQuery = slaQuery;
    }

    public async Task<ServiceCategoryDetailsModel[]> ListAsync(
        ServiceCategoryFilter filter,
        CancellationToken cancellationToken = default)
    {
        var targetUserId = filter.UserID ?? _currentUser.UserId;
        var user = await _userFinder
            .With(x => x.Subdivision)
            .FindAsync(targetUserId, cancellationToken);

        if (user == null)
            throw new ObjectNotFoundException($"User (ID = {targetUserId})");

        var availableSla = await _slaQuery.GetByUserAsync(targetUserId, cancellationToken);
        var filteredSla = availableSla
            .Where(sla => sla.UtcFinishDate == null || sla.UtcFinishDate > DateTime.UtcNow);
        var slaReferences = await _slaReferences.ToArrayAsync(
            slaRef => filteredSla.Select(sla => sla.ID).Contains(slaRef.SLAID),
            cancellationToken);
        var customControls = await _customControls.ToArrayAsync(
            x => x.UserId == targetUserId
                && new[] { ObjectClass.ServiceItem, ObjectClass.ServiceAttendance }.Contains(x.ObjectClass),
            cancellationToken);

        Expression<Func<ServiceCategory, bool>> categoriesFilter = x => true;
        if (filter.CategoryID.HasValue)
            categoriesFilter = categoriesFilter.And(x => x.ID == filter.CategoryID);
        var categories = await _repository.ToArrayAsync(categoriesFilter, cancellationToken);

        Expression<Func<Service, bool>> servicesFilter = filter.ServiceTypes != null && filter.ServiceTypes.Any()
            ? service => filter.ServiceTypes.Contains(service.Type) && (service.State == CatalogItemState.Worked || service.State == CatalogItemState.Blocked)
            : Service.IsExternal.And(Service.WorkedOrBlocked);
        if (filter.ServiceID.HasValue)
            servicesFilter = servicesFilter.And(x => x.ID == filter.ServiceID);

        var services = await _services
                .With(x => x.Category)
                .ToArrayAsync(servicesFilter, cancellationToken);

        var serviceItemsFilter = PortfolioServiceItemAbstract.WorkedOrBlocked<ServiceItem>()
            .And(ServiceItem.ServiceIsExternal)
            .And(ServiceItem.ServiceWorkedOrBlocked);
        if (filter.ServiceItemAttendanceID.HasValue)
            serviceItemsFilter = serviceItemsFilter.And(x => x.ID == filter.ServiceItemAttendanceID);

        var serviceItems = await _serviceItems
            .With(x => x.Service)
                .ThenWith(x => x.Category)
            .ToArrayAsync(
                serviceItemsFilter,
                cancellationToken);

        var serviceAttendancesFilter = PortfolioServiceItemAbstract.WorkedOrBlocked<ServiceAttendance>()
                    .And(ServiceAttendance.ServiceIsExternal)
                    .And(ServiceAttendance.ServiceWorkedOrBlocked)
                    .And(x => x.Type == AttendanceType.User);
        if (filter.ServiceItemAttendanceID.HasValue)
            serviceAttendancesFilter = serviceAttendancesFilter.And(x => x.ID == filter.ServiceItemAttendanceID);

        var serviceAttendances = await _serviceAttendances
            .With(x => x.Service)
                .ThenWith(x => x.Category)
            .ToArrayAsync(
                serviceAttendancesFilter,
                cancellationToken);

        var serviceItemList = serviceItems
            .Select(
                serviceItem =>
                {
                    var model = _mapper.Map<ServiceItemDetailsModel>(serviceItem);
                    model.IsAvailable = slaReferences.Any(x => x.ObjectID == serviceItem.ID);
                    model.IsInFavorite = customControls
                        .Any(x => x.ObjectId == serviceItem.ID
                            && x.ObjectClass == ObjectClass.ServiceItem);
                    return model;
                })
            .Union(
                serviceAttendances
                    .Select(
                        serviceAttendance =>
                        {
                            var model = _mapper.Map<ServiceItemDetailsModel>(serviceAttendance);
                            model.IsAvailable = slaReferences.Any(x => x.ObjectID == serviceAttendance.ID);
                            model.IsInFavorite = customControls
                                .Any(x => x.ObjectId == serviceAttendance.ID
                                    && x.ObjectClass == ObjectClass.ServiceAttendance);
                            return model;
                        }))
            .ToArray();

        var serviceModels = services.Select(s => _mapper.Map<ServiceDetailsModel>(s)).ToArray();
        serviceModels.ForEach(service =>
        {
            filteredSla.ForEach(sla =>
            {
                if (slaReferences.Any(slaRef => slaRef.SLAID == sla.ID && slaRef.ObjectID == service.ID))
                {
                    service.IsAllItemsAvailableByDefault |= !serviceItemList.Any(item => 
                        item.ServiceID == service.ID && 
                        slaReferences.Any(slaRef => slaRef.SLAID == sla.ID && slaRef.ObjectID == item.ID));
                }
            });
        });
        
        return categories
            .Select(
                cat =>
                {
                    var model = _mapper.Map<ServiceCategoryDetailsModel>(cat);
                    model.ServiceList = serviceModels.Where(x => x.ServiceCategoryID == cat.ID)
                        .Where(x => !filter.AvailableOnly || x.IsAvailable)
                        .OrderBy(x => x.Name)
                        .ToArray();
                    serviceModels.ForEach(
                        service =>
                        {
                            var serviceModel = _mapper.Map<ServiceDetailsModel>(service);
                            serviceModel.ServiceItemAttendanceList = serviceItemList
                                .Where(x => x.ServiceID == service.ID)
                                .Where(x => !filter.AvailableOnly || x.IsAvailable ||
                                            service.IsAllItemsAvailableByDefault)
                                .OrderBy(x => x.ClassID)
                                .ThenBy(x => x.Name)
                                .ToArray();
                            var serviceAvailable = slaReferences.Any(x => x.ObjectID == service.ID);
                            if (serviceAvailable)
                                serviceModel.ServiceItemAttendanceList.ForEach(x => x.IsAvailable = true);

                            serviceModel.IsAvailable = serviceModel.ServiceItemAttendanceList.Any(sd => sd.IsAvailable) || serviceAvailable;
                        });
                    model.IsAvailable = model.ServiceList.Any(sd => sd.IsAvailable);
                    return model;
                })
            .OrderBy(x => x.Name)
            .ToArray();
    }
}