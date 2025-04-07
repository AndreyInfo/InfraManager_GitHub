using AutoMapper;
using InfraManager.BLL.Software.SoftwareModels;
using InfraManager.DAL.Software;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software;

public class AnotherSoftwareModelProvider<T> : ISoftwareModelProvider where T : SoftwareModelDetailsBase
{
    private readonly IMapper _mapper;

    public AnotherSoftwareModelProvider(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<SoftwareModelDetailsBase> GetAsync(SoftwareModel model, SoftwareModelTemplate modelTemplate)
    {
        var details = _mapper.Map<T>(model);
        return Task.FromResult(details as SoftwareModelDetailsBase);
    }
}
