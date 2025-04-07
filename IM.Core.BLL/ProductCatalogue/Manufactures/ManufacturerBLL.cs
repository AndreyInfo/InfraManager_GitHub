using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using Inframanager.BLL.AccessManagement;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ProductCatalogue.Manufactures;

internal class ManufacturerBLL : 
    StandardBLL<int, Manufacturer, ManufacturerData, ManufacturerDetails, ManufacturersFilter>
    , IManufacturersBLL
    , ISelfRegisteredService<IManufacturersBLL>
{
    private readonly IMapper _mapper;
    private readonly IValidatePermissions<Manufacturer> _validatePermissions;
    private readonly IGuidePaggingFacade<Manufacturer, ManufacturerColumns> _guidePaggingFacade; 
    private readonly IBuildEntityQuery<Manufacturer, ManufacturerDetails, ManufacturersFilter> _buildEntityQuery;
    public ManufacturerBLL(IRepository<Manufacturer> repository
        , ILogger<ManufacturerBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<ManufacturerDetails, Manufacturer> detailsBuilder
        , IInsertEntityBLL<Manufacturer, ManufacturerData> insertEntityBLL
        , IModifyEntityBLL<int, Manufacturer, ManufacturerData, ManufacturerDetails> modifyEntityBLL
        , IRemoveEntityBLL<int, Manufacturer> removeEntityBLL
        , IGetEntityBLL<int, Manufacturer, ManufacturerDetails> detailsBLL
        , IGetEntityArrayBLL<int, Manufacturer, ManufacturerDetails, ManufacturersFilter> detailsArrayBLL
        , IMapper mapper
        , IValidatePermissions<Manufacturer> validatePermissions
        , IGuidePaggingFacade<Manufacturer, ManufacturerColumns> guidePaggingFacade
        , IBuildEntityQuery<Manufacturer, ManufacturerDetails, ManufacturersFilter> buildEntityQuery)
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
        _validatePermissions = validatePermissions;
        _guidePaggingFacade = guidePaggingFacade;
        _buildEntityQuery = buildEntityQuery;
    }

    public async Task<ManufacturerDetails[]> GetPaggingAsync(ManufacturersFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(Logger, CurrentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var manufacturers = await _guidePaggingFacade.GetPaggingAsync(filter
            , _buildEntityQuery.Query(filter)
            , c => c.Name.ToLower().Contains(filter.SearchString.ToLower())
                    || c.ExternalID.ToLower().Contains(filter.SearchString.ToLower())
            , cancellationToken);

        return _mapper.Map<ManufacturerDetails[]>(manufacturers);
    }
}