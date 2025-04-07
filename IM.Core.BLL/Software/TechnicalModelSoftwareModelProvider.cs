using AutoMapper;
using InfraManager.BLL.Software.SoftwareModels;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.DAL.Software;
using System;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software;

public class TechnicalModelSoftwareModelProvider : ISoftwareModelProvider
{
    private readonly IMapper _mapper;

    public TechnicalModelSoftwareModelProvider(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<SoftwareModelDetailsBase> GetAsync(SoftwareModel model, SoftwareModelTemplate modelTemplate)
    {
        var details = _mapper.Map<SoftwareTechnicalModelDetails>(model);

        return Task.FromResult((SoftwareModelDetailsBase)details);
    }
}
