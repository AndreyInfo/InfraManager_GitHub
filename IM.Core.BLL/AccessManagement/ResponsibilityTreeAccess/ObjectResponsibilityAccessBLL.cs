using AutoMapper;
using InfraManager.BLL.OrganizationStructure.Groups;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.AccessManagement.ResponsibilityTreeAccess;

internal class ObjectResponsibilityAccessBLL : IObjectResponsibilityAccessBLL
    , ISelfRegisteredService<IObjectResponsibilityAccessBLL>
{
    private readonly IRepository<ObjectAccess> _objectAccesses;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceMapper<(AccessTypes, ObjectClass), ISaveAccess> _serviceMapper;
    public ObjectResponsibilityAccessBLL(IRepository<ObjectAccess> objectAccesses
                           , IMapper mapper
                           , IUnitOfWork unitOfWork
                           , IServiceMapper<(AccessTypes, ObjectClass), ISaveAccess> serviceMapper)
    {
        _objectAccesses = objectAccesses;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _serviceMapper = serviceMapper;
    }


    public async Task<ItemResponsibilityTrees> GetAccessAsync(Guid ownerID, CancellationToken cancellationToken)
    {
        return new ItemResponsibilityTrees
        {
            DeviceCatalogueElements = await GetObjectsAccessAsync(ownerID, AccessTypes.DeviceCatalogue, cancellationToken),
            ServiceCatalogueElements = await GetObjectsAccessAsync(ownerID, AccessTypes.ServiceCatalogue, cancellationToken),
            TTZ_sksElements = await GetObjectsAccessAsync(ownerID, AccessTypes.TTZ_sks, cancellationToken),
            TOZ_sksElements = await GetObjectsAccessAsync(ownerID, AccessTypes.TOZ_sks, cancellationToken),
            TOZ_orgElements = await GetObjectsAccessAsync(ownerID, AccessTypes.TOZ_org, cancellationToken),
        };
    }

    private async Task<AccessElementsDetails[]> GetObjectsAccessAsync(Guid ownerID, AccessTypes accessTypes, CancellationToken cancellationToken)
    {
        var objectAccesses = await _objectAccesses.ToArrayAsync(c => c.OwnerID == ownerID
                                                                     && c.Type == accessTypes
                                                                     , cancellationToken);

        return _mapper.Map<AccessElementsDetails[]>(objectAccesses);
    }


    public async Task DeleteByOwnerAsync(Guid ownerID, CancellationToken cancellationToken)
    {
        var objectsPermission = await _objectAccesses.ToArrayAsync(c => c.OwnerID == ownerID, cancellationToken);
        objectsPermission.ForEach(c => _objectAccesses.Delete(c));
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task SaveAccess(AccessElementsDetails[] models, CancellationToken cancellationToken)
    {
        using var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadUncommitted, TransactionScopeOption.Suppress);
        foreach (var model in models)
        {
            await _serviceMapper.Map((model.AccessType, model.ObjectClassID))
                                .SaveAccessAsync(model, model.ID, cancellationToken);
        }
        transaction.Complete();
    }
}