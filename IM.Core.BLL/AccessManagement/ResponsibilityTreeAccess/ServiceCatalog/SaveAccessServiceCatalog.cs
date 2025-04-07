using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.ServiceCatalog;

internal sealed class SaveAccessServiceCatalog : SaveAccessAbstract<Owner>
{
    private readonly IReadonlyRepository<Service> _services;
    private readonly IReadonlyRepository<ServiceCategory> _serviceCategories;
    private readonly IReadonlyRepository<ServiceAttendance> _serviceAttendances;
    private readonly IReadonlyRepository<ServiceItem> _serviceItems;

    protected override AccessTypes AccessType => AccessTypes.ServiceCatalogue;
    protected override ObjectClass ClassID => ObjectClass.ServiceCatalogue;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[]
    {
        ObjectClass.ServiceCategory,
        ObjectClass.Service,
        ObjectClass.ServiceItem,
        ObjectClass.ServiceAttendance,
    };


    public SaveAccessServiceCatalog(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<ServiceCategory> serviceCategories
        , IReadonlyRepository<Service> services
        , IReadonlyRepository<ServiceAttendance> serviceAttendances
        , IReadonlyRepository<ServiceItem> serviceItems)
        : base(objectAccesses
            , mapper
            , unitOfWork)

    {
        _serviceCategories = serviceCategories;
        _services = services;
        _serviceAttendances = serviceAttendances;
        _serviceItems = serviceItems;
    }

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
    {
        await InsertAllServiceCategoriesAccessAsync(ownerID, cancellationToken);
        await InsertAllServicesAccessAsync(ownerID, cancellationToken);
        await InsertAllServiceItemsAccessAsync(ownerID, cancellationToken);
        await InsertAllServiceAttendancesAccessAsync(ownerID, cancellationToken);
    }

    private async Task InsertAllServiceCategoriesAccessAsync(Guid ownerID, CancellationToken cancellationToken)
    {
        var serviceCategories = await _serviceCategories.ToArrayAsync(cancellationToken);
        foreach (var catrogty in serviceCategories)
            await InsertItemAsync(ownerID, catrogty.ID, ObjectClass.ServiceCategory, cancellationToken: cancellationToken);
    }

    private async Task InsertAllServicesAccessAsync(Guid ownerID, CancellationToken cancellationToken)
    {
        var services = await _services.ToArrayAsync(cancellationToken);
        foreach (var service in services)
            await InsertItemAsync(ownerID, service.ID, ObjectClass.Service, cancellationToken: cancellationToken);
    }

    private async Task InsertAllServiceItemsAccessAsync(Guid ownerID, CancellationToken cancellationToken)
    {
        var services = await _serviceItems.ToArrayAsync(cancellationToken);
        foreach (var service in services)
            await InsertItemAsync(ownerID, service.ID, ObjectClass.ServiceItem, cancellationToken: cancellationToken);
    }

    private async Task InsertAllServiceAttendancesAccessAsync(Guid ownerID, CancellationToken cancellationToken)
    {
        var services = await _serviceAttendances.ToArrayAsync(cancellationToken);
        foreach (var service in services)
            await InsertItemAsync(ownerID, service.ID, ObjectClass.ServiceAttendance, cancellationToken: cancellationToken);
    }

    protected override Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
    {
        return Task.FromResult(Array.Empty<Guid>());
    }
}