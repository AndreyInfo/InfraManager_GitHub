using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ColumnMapper;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Asset.Peripherals;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.Peripherals;

internal class PeripheralBLL : EquipmentBaseBLL<Guid, Peripheral, PeripheralData, PeripheralDetails>
    , IPeripheralBLL
    , ISelfRegisteredService<IPeripheralBLL>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Peripheral> _peripheralRepository;
    private readonly IOrderedColumnQueryBuilder<Peripheral, PeripheralColumns> _peripheralOrderedColumnQueryBuilder;
    private readonly IPeripheralQuery _peripheralQuery;

    public PeripheralBLL(
        IRepository<Peripheral> repository
        , ILogger<PeripheralBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<PeripheralDetails, Peripheral> detailsBuilder
        , IInsertEntityBLL<Peripheral, PeripheralData> insertEntityBLL
        , IModifyEntityBLL<Guid, Peripheral, PeripheralData, PeripheralDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, Peripheral> removeEntityBLL
        , IGetEntityBLL<Guid, Peripheral, PeripheralDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, Peripheral, PeripheralDetails, BaseFilter> detailsArrayBLL
        , IMapper mapper
        , IAssetBLL assetBLL
        , IRepository<Peripheral> peripheralRepository
        , IOrderedColumnQueryBuilder<Peripheral, PeripheralColumns> peripheralOrderedColumnQueryBuilder
        , IPeripheralQuery peripheralQuery
        )
        : base(repository
            , logger
            , unitOfWork
            , currentUser
            , detailsBuilder
            , insertEntityBLL
            , modifyEntityBLL
            , removeEntityBLL
            , detailsBLL
            , detailsArrayBLL
            , mapper
            , assetBLL)
    {
        _mapper = mapper;
        _peripheralRepository = peripheralRepository;
        _peripheralOrderedColumnQueryBuilder = peripheralOrderedColumnQueryBuilder;
        _peripheralQuery = peripheralQuery;
    }

    public async Task<PeripheralListItemDetails[]> GetPeripheralsForNetworkDeviceAsync(int networkDeviceID, BaseFilter filter, CancellationToken cancellationToken)
    {
        var query = _peripheralRepository.Query().Where(x => x.NetworkDeviceID == networkDeviceID);

        var orderedQuery = await _peripheralOrderedColumnQueryBuilder.BuildQueryAsync(filter.ViewName, query, cancellationToken);

        var adapterItems = await _peripheralQuery.ExecuteAsync(_mapper.Map<PaggingFilter>(filter), orderedQuery, cancellationToken);

        return _mapper.Map<PeripheralListItemDetails[]>(adapterItems);
    }
}