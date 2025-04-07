using System;
using Inframanager.BLL;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Units;
using Microsoft.Extensions.Logging;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Threading.Tasks;
using System.Threading;
using Inframanager;
using AutoMapper;
using Inframanager.BLL.AccessManagement;

namespace InfraManager.BLL.ProductCatalogue.Units;

internal class UnitBLL : StandardBLL<Guid, Unit, UnitData, UnitDetails, BaseFilter>
    , IUnitBLL
    , ISelfRegisteredService<IUnitBLL>
{
    private readonly IMapper _mapper;
    private readonly IValidatePermissions<Unit> _validatePermissions;
    private readonly IGuidePaggingFacade<Unit, UnitColumns> _guidePaggingFacade;

    public UnitBLL(IRepository<Unit> repository
        , IMapper mapper
        , ILogger<UnitBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IRemoveEntityBLL<Guid, Unit> removeEntityBLL
        , IBuildObject<UnitDetails, Unit> detailsBuilder
        , IValidatePermissions<Unit> validatePermissions
        , IInsertEntityBLL<Unit, UnitData> insertEntityBLL
        , IGetEntityBLL<Guid, Unit, UnitDetails> detailsBLL
        , IGuidePaggingFacade<Unit, UnitColumns> guidePaggingFacade
        , IModifyEntityBLL<Guid, Unit, UnitData, UnitDetails> modifyEntityBLL
        , IGetEntityArrayBLL<Guid, Unit, UnitDetails, BaseFilter> detailsArrayBLL) 
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
        _guidePaggingFacade = guidePaggingFacade;
        _validatePermissions = validatePermissions;
    }

    public async Task<UnitDetails[]> GetListAsync(BaseFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var units = await _guidePaggingFacade.GetPaggingAsync(filter
            , Repository.Query()
            , x => x.Name.ToLower().Contains(filter.SearchString.ToLower())
            , cancellationToken);

        return _mapper.Map<UnitDetails[]>(units);
    }
}