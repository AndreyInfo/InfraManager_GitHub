using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.ServiceCatalog;
internal abstract class SaveAccessServiceCatalogAbstract<TEntity>
    : SaveAccessAbstract<TEntity>
    where TEntity : class
{
    private readonly IReadonlyRepository<Service> _services;
    private readonly IReadonlyRepository<ServiceItem> _serviceItems;
    private readonly IReadonlyRepository<ServiceAttendance> _serviceAttendances;
    protected override AccessTypes AccessType => AccessTypes.ServiceCatalogue;

    protected SaveAccessServiceCatalogAbstract(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Service> services
        , IReadonlyRepository<ServiceItem> serviceItems
        , IReadonlyRepository<ServiceAttendance> serviceAttendances)
        : base(objectAccesses
            , mapper
            , unitOfWork)
    {
        _services = services;
        _serviceItems = serviceItems;
        _serviceAttendances = serviceAttendances;
    }

    #region Получение id вложенных элементов
    protected async Task<Guid[]> GetCategorySubObjectsIDAsync(Guid categoryID, CancellationToken cancellationToken)
    {
        var subObjectID = new List<Guid>();

        var services = await _services.ToArrayAsync(c => c.CategoryID == categoryID, cancellationToken);
        services.ForEach(c => subObjectID.Add(c.ID));

        var servicesID = services.Select(c => c.ID).ToArray();
        subObjectID.AddRange(await GetServicesSubObjectsIDAsync(servicesID, cancellationToken));

        return subObjectID.ToArray();
    }

    protected async Task<Guid[]> GetServicesSubObjectsIDAsync(Guid[] servicesID, CancellationToken cancellationToken)
    {
        var subObjectID = new List<Guid>(await GetServiceItemsIDAsync(servicesID, cancellationToken));
        subObjectID.AddRange(await GetServiceAttendancesIDAsync(servicesID, cancellationToken));
        return subObjectID.ToArray();
    }

    private async Task<Guid[]> GetServiceItemsIDAsync(Guid[] servicesID, CancellationToken cancellationToken)
    {
        var serviceItems = await _serviceItems.ToArrayAsync(c => c.ServiceID.HasValue
                                                          && servicesID.Contains(c.ServiceID.Value)
                                                          , cancellationToken);

        return serviceItems.Select(c => c.ID).ToArray();
    }

    private async Task<Guid[]> GetServiceAttendancesIDAsync(Guid[] servicesID, CancellationToken cancellationToken)
    {
        var serviceAttendances = await _serviceAttendances.ToArrayAsync(c => c.ServiceID.HasValue
                                                      && servicesID.Contains(c.ServiceID.Value)
                                                      , cancellationToken);

        return serviceAttendances.Select(c => c.ID).ToArray();
    }
    #endregion


    #region Добавление вложенных элементов
    protected async Task InsertSubObjectsCategoryAccessAsync(Guid categoryID, Guid ownerID, CancellationToken cancellationToken)
    {
        var buildings = await _services.ToArrayAsync(c => c.CategoryID == categoryID, cancellationToken);
        foreach (var service in buildings)
        {
            await InsertSubObjectsServiceAsync(service.ID, ownerID, cancellationToken);
            await InsertItemAsync(ownerID, service.ID, ObjectClass.Service, cancellationToken: cancellationToken);
        }
    }

    protected async Task InsertSubObjectsServiceAsync(Guid serviceID, Guid ownerID, CancellationToken cancellationToken)
    {
        await InsertServiceItemsAccessAsync(serviceID, ownerID, cancellationToken);
        await InsertServiceAttendancesAccessAsync(serviceID, ownerID, cancellationToken);
    }

    private async Task InsertServiceItemsAccessAsync(Guid serviceID, Guid ownerID, CancellationToken cancellationToken)
    {
        var serviceItems = await _serviceItems.ToArrayAsync(c => c.ServiceID == serviceID, cancellationToken);
        foreach (var floor in serviceItems)
            await InsertItemAsync(ownerID, floor.ID, ObjectClass.ServiceItem, cancellationToken: cancellationToken);
    }

    private async Task InsertServiceAttendancesAccessAsync(Guid serviceID, Guid ownerID, CancellationToken cancellationToken)
    {
        var serviceAttendances = await _serviceAttendances.ToArrayAsync(c => c.ServiceID == serviceID, cancellationToken);
        foreach (var serviceAttendance in serviceAttendances)
            await InsertItemAsync(ownerID, serviceAttendance.ID, ObjectClass.ServiceAttendance, cancellationToken: cancellationToken);
    }
    #endregion
}
