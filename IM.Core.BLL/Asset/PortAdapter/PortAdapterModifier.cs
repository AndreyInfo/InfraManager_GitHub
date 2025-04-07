using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.ResourcesArea;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PortAdapterEntity = InfraManager.DAL.Asset.PortAdapter;

namespace InfraManager.BLL.Asset.PortAdapter;

internal sealed class PortAdapterModifier :
    IModifyObject<PortAdapterEntity, PortAdapterData>,
        ISelfRegisteredService<IModifyObject<PortAdapterEntity, PortAdapterData>>
{
    private readonly IMapper _mapper;
    private readonly IRepository<PortAdapterEntity> _repository;
    private readonly ILocalizeText _localizeText;

    public PortAdapterModifier(
          IMapper mapper
        , IRepository<PortAdapterEntity> repository
        , ILocalizeText localizeText
        )
    {
        _mapper = mapper;
        _repository = repository;
        _localizeText = localizeText;
    }

    public async Task ModifyAsync(PortAdapterEntity entity, PortAdapterData data, CancellationToken cancellationToken = default)
    {
        if (entity.PortNumber != data.PortNumber)
        {
            if (_repository.Any(x => x.ObjectID == data.ObjectID && x.PortNumber == data.PortNumber))
                throw new InvalidObjectException(string.Format(_localizeText.Localize(nameof(Resources.ValidationErrorType_AlreadyExist)), data.ObjectID, data.PortNumber));
        }
        _mapper.Map(data, entity);
    }

    public void SetModifiedDate(PortAdapterEntity entity)
    {
    }
}

