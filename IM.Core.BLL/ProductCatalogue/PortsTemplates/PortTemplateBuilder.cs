using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.BLL.ProductCatalogue.PortTemplates;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.ResourcesArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.PortsTemplates;

internal class PortTemplateBuilder : IBuildObject<PortTemplate, PortTemplatesData>
    , ISelfRegisteredService<IBuildObject<PortTemplate, PortTemplatesData>>
{
    private readonly IMapper _mapper;
    private readonly ILocalizeText _localizeText;
    private readonly IReadonlyRepository<PortTemplate> _repository;

    public PortTemplateBuilder(IMapper mapper
        , ILocalizeText localizeText
        , IReadonlyRepository<PortTemplate> repository)
    {
        _mapper = mapper;
        _repository = repository;
        _localizeText = localizeText;
    }

    public async Task<PortTemplate> BuildAsync(PortTemplatesData data, CancellationToken cancellationToken = default)
    {
        if (_repository.Any(x => x.ObjectID == data.ObjectID && x.PortNumber == data.PortNumber))
            throw new InvalidObjectException(string.Format(_localizeText.Localize(nameof(Resources.ValidationErrorType_AlreadyExist)), data.ObjectID, data.PortNumber));

        return _mapper.Map<PortTemplate>(data);
    }

    public Task<IEnumerable<PortTemplate>> BuildManyAsync(IEnumerable<PortTemplatesData> dataItems, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

