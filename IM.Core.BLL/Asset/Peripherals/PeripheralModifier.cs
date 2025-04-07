using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System.Threading;
using System.Threading.Tasks;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset.Peripherals;

internal sealed class PeripheralModifier : IModifyObject<Peripheral, PeripheralData>
, ISelfRegisteredService<IModifyObject<Peripheral, PeripheralData>>
{
    private readonly IMapper _mapper;
    private readonly IFinder<AssetEntity> _assetFinder;

    public PeripheralModifier(
          IMapper mapper
        , IFinder<AssetEntity> assetFinder)
    {
        _mapper = mapper;
        _assetFinder = assetFinder;
    }

    public async Task ModifyAsync(Peripheral entity, PeripheralData data, CancellationToken cancellationToken = default)
    {
        _mapper.Map(data, entity);

        var asset = await _assetFinder.FindAsync(entity.IMObjID, cancellationToken);
        var assetData = _mapper.Map<AssetData>(entity);
        _mapper.Map(data, assetData);
        _mapper.Map(assetData, asset);
    }

    public void SetModifiedDate(Peripheral entity)
    {
    }
}
