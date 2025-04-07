using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.DAL;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using SupplierContactPersonEntity = InfraManager.DAL.Suppliers.SupplierContactPerson;

namespace InfraManager.BLL.Suppliers.SupplierContactPerson;

internal class SupplierContactPersonBLL 
    : StandardBLL<Guid, SupplierContactPersonEntity, SupplierContactPersonData, SupplierContactPersonDetails, SupplierContactPersonFilter>
    , ISupplierContactPersonBLL
    , ISelfRegisteredService<ISupplierContactPersonBLL>
{
    private readonly IMapper _mapper;
    private readonly IValidatePermissions<SupplierContactPersonEntity> _validatePermissions;
    private readonly IGuidePaggingFacade<SupplierContactPersonEntity, SupplierContactPersonColumns> _guidePaggingFacade;
    private readonly IBuildEntityQuery<SupplierContactPersonEntity, SupplierContactPersonDetails, SupplierContactPersonFilter> _buildEntityQuery;

    public SupplierContactPersonBLL(
          IRepository<SupplierContactPersonEntity> repository
        , IMapper mapper
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , ILogger<SupplierContactPersonBLL> logger
        , IValidatePermissions<SupplierContactPersonEntity> validatePermissions
        , IBuildObject<SupplierContactPersonDetails, SupplierContactPersonEntity> detailsBuilder
        , IInsertEntityBLL<SupplierContactPersonEntity, SupplierContactPersonData> insertEntityBLL
        , IRemoveEntityBLL<Guid, SupplierContactPersonEntity> removeEntityBLL
        , IGuidePaggingFacade<SupplierContactPersonEntity, SupplierContactPersonColumns> guidePaggingFacade
        , IGetEntityBLL<Guid, SupplierContactPersonEntity, SupplierContactPersonDetails> detailsBLL
        , IBuildEntityQuery<SupplierContactPersonEntity, SupplierContactPersonDetails, SupplierContactPersonFilter> buildEntityQuery
        , IGetEntityArrayBLL<Guid, SupplierContactPersonEntity, SupplierContactPersonDetails, SupplierContactPersonFilter> detailsArrayBLL
        , IModifyEntityBLL<Guid, SupplierContactPersonEntity, SupplierContactPersonData, SupplierContactPersonDetails> modifyEntityBLL)
        : base(repository
            , logger
            , unitOfWork
            , currentUser
            , detailsBuilder
            , insertEntityBLL
            , modifyEntityBLL
            , removeEntityBLL
            , detailsBLL
            , detailsArrayBLL)
    {
        _mapper = mapper;
        _buildEntityQuery = buildEntityQuery;
        _guidePaggingFacade = guidePaggingFacade;
        _validatePermissions = validatePermissions;
    }

    public async Task<SupplierContactPersonDetails[]> GetListAsync(SupplierContactPersonFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var contacts = await _guidePaggingFacade.GetPaggingAsync(filter
            , _buildEntityQuery.Query(filter)
            , null
            , cancellationToken);

        return _mapper.Map<SupplierContactPersonDetails[]>(contacts);
    }
}