using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using InfraManager.ResourcesArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles;
internal sealed class LifeCycleBuilder :
     IBuildObject<LifeCycle, LifeCycleData>,
     ISelfRegisteredService<IBuildObject<LifeCycle, LifeCycleData>>
{
    private readonly IMapper _mapper;
    private readonly ILocalizeText _localizeText;
    private const int AllowedDefaultStatesCount = 1;
    
    public LifeCycleBuilder(IMapper mapper,
        ILocalizeText localizeText )
    {
        _mapper = mapper;
        _localizeText = localizeText;
    }

    public async Task<LifeCycle> BuildAsync(LifeCycleData data, CancellationToken cancellationToken = default)
    {
        if (data.States.Count(c => c.IsDefault) > AllowedDefaultStatesCount)
            throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.ErrorSeveralDefaultLifeCycleState), cancellationToken));

        return _mapper.Map<LifeCycle>(data);
    }

    public Task<IEnumerable<LifeCycle>> BuildManyAsync(IEnumerable<LifeCycleData> dataItems, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
