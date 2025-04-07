using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ColumnMapper;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Asset.Adapters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.Adapters;

internal class AdapterBLL : EquipmentBaseBLL<Guid, Adapter, AdapterData, AdapterDetails>
    , IAdapterBLL
    , ISelfRegisteredService<IAdapterBLL>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Adapter> _adapterRepository;
    private readonly IOrderedColumnQueryBuilder<Adapter, AdapterColumns> _adapterOrderedColumnQueryBuilder;
    private readonly IAdapterQuery _adapterQuery;

    public AdapterBLL(
          IRepository<Adapter> repository
        , ILogger<AdapterBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<AdapterDetails, Adapter> detailsBuilder
        , IInsertEntityBLL<Adapter, AdapterData> insertEntityBLL
        , IModifyEntityBLL<Guid, Adapter, AdapterData, AdapterDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, Adapter> removeEntityBLL
        , IGetEntityBLL<Guid, Adapter, AdapterDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, Adapter, AdapterDetails, BaseFilter> detailsArrayBLL
        , IMapper mapper
        , IAssetBLL assetBLL
        , IRepository<Adapter> adapterRepository
        , IOrderedColumnQueryBuilder<Adapter, AdapterColumns> adapterOrderedColumnQueryBuilder
        , IAdapterQuery adapterQuery
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
            , assetBLL
            )
    {
        _mapper = mapper;
        _adapterRepository = adapterRepository;
        _adapterOrderedColumnQueryBuilder = adapterOrderedColumnQueryBuilder;
        _adapterQuery = adapterQuery;
    }

    public async Task<AdapterListItemDetails[]> GetAdaptersForNetworkDeviceAsync(int networkDeviceID, BaseFilter filter, CancellationToken cancellationToken)
    {
        var query = _adapterRepository.Query().Where(x => x.NetworkDeviceID == networkDeviceID);

        var orderedQuery = await _adapterOrderedColumnQueryBuilder.BuildQueryAsync(filter.ViewName, query, cancellationToken);

        var adapterItems = await _adapterQuery.ExecuteAsync(_mapper.Map<PaggingFilter>(filter), orderedQuery, cancellationToken);

        return _mapper.Map<AdapterListItemDetails[]>(adapterItems);
    }
}