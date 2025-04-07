using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.ServiceCatalog;
internal sealed class SaveAccessServiceCategory : SaveAccessServiceCatalogAbstract<ServiceCategory>
{
    protected override ObjectClass ClassID => ObjectClass.ServiceCategory;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => new ObjectClass[]
    {
        ObjectClass.Service,
        ObjectClass.ServiceItem,
        ObjectClass.ServiceAttendance,
    };

    public SaveAccessServiceCategory(IRepository<ObjectAccess> objectAccesses
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , IReadonlyRepository<Service> services
        , IReadonlyRepository<ServiceItem> serviceItems
        , IReadonlyRepository<ServiceAttendance> serviceAttendances)
        : base(objectAccesses
            , mapper
            , unitOfWork
            , services
            , serviceItems
            , serviceAttendances)
    {
    }

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
        => await InsertSubObjectsCategoryAccessAsync(parentID, ownerID, cancellationToken);


    protected override async Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
        => await GetCategorySubObjectsIDAsync(parentID, cancellationToken);
}
