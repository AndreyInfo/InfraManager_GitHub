using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.ResourcesArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PortAdapterEntity = InfraManager.DAL.Asset.PortAdapter;

namespace InfraManager.BLL.Asset.PortAdapter;

internal sealed class PortAdapterBuilder : IBuildObject<PortAdapterEntity, PortAdapterData>
    , ISelfRegisteredService<IBuildObject<PortAdapterEntity, PortAdapterData>>
{
    private readonly IMapper _mapper;
    private readonly ILocalizeText _localizeText;
    private readonly IReadonlyRepository<PortAdapterEntity> _repository;

    public PortAdapterBuilder(
          IMapper mapper
        , ILocalizeText localizeText
        , IReadonlyRepository<PortAdapterEntity> repository
        )
    {
        _mapper = mapper;
        _repository = repository;
        _localizeText = localizeText;
    }

    public async Task<PortAdapterEntity> BuildAsync(PortAdapterData data, CancellationToken cancellationToken = default)
    {
        if (_repository.Any(x => x.ObjectID == data.ObjectID && x.PortNumber == data.PortNumber))
            throw new InvalidObjectException(string.Format(_localizeText.Localize(nameof(Resources.ValidationErrorType_AlreadyExist)), data.ObjectID, data.PortNumber));

        return _mapper.Map<PortAdapterEntity>(data);
    }

    public Task<IEnumerable<PortAdapterEntity>> BuildManyAsync(IEnumerable<PortAdapterData> dataItems, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

