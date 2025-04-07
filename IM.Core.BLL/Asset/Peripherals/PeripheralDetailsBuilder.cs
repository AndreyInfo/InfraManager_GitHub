using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetEntity = InfraManager.DAL.Asset.Asset;

namespace InfraManager.BLL.Asset.Peripherals;

internal sealed class PeripheralDetailsBuilder : IBuildObject<PeripheralDetails, Peripheral>
    , ISelfRegisteredService<IBuildObject<PeripheralDetails, Peripheral>>
{
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<AssetEntity> _assetRepository;

    public PeripheralDetailsBuilder(
            IMapper mapper
          , IReadonlyRepository<AssetEntity> assetRepository
        )
    {
        _mapper = mapper;
        _assetRepository = assetRepository;
    }

    public async Task<PeripheralDetails> BuildAsync(Peripheral entity, CancellationToken cancellationToken = default)
    {
        var peripheral = _mapper.Map<PeripheralDetails>(entity);

        var asset = await _assetRepository.With(x => x.User).ThenWith(x => x.Workplace)
            .With(x => x.Owner).With(x => x.Utilizer)
            .FirstOrDefaultAsync(x => x.ID == peripheral.IMObjID, cancellationToken);

        _mapper.Map(asset, peripheral);

        peripheral.Owner = asset.Owner.FullName;
        peripheral.Utilizer = asset.Utilizer.FullName;
        peripheral.Location = GetLocation(peripheral, asset.User, cancellationToken);

        return peripheral;
    }

    private string GetLocation(PeripheralDetails peripheral, User user, CancellationToken cancellationToken)
    {
        string sep = "/";
        return string.Concat(peripheral.OrganizationName, sep, peripheral.BuildingName, sep,
            peripheral.FloorName, sep, peripheral.RoomName, sep, user.Workplace.Name);
    }

    public Task<IEnumerable<PeripheralDetails>> BuildManyAsync(IEnumerable<Peripheral> dataItems, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}