using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess.ServiceCatalog;
internal sealed class SaveAccessServiceItem : SaveAccessServiceCatalogAbstract<ServiceItem>
{
    protected override ObjectClass ClassID => ObjectClass.ServiceItem;
    protected override IReadOnlyCollection<ObjectClass> ChildClasses => Array.Empty<ObjectClass>();

    public SaveAccessServiceItem(IRepository<ObjectAccess> objectAccesses
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


    protected override Task<Guid[]> GetIDsSubObjectsAsync(Guid parentID, CancellationToken cancellationToken)
        => Task.FromResult(Array.Empty<Guid>());

    protected override async Task InsertSubObjectsAsync(Guid parentID, Guid ownerID, CancellationToken cancellationToken)
    {
    }
}
