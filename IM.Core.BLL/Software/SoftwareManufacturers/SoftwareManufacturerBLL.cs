using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareManufacturers;

internal class SoftwareManufacturerBLL : 
    ISoftwareManufacturerBLL
    , ISelfRegisteredService<ISoftwareManufacturerBLL>
{
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<Manufacturer> _repository;
    public SoftwareManufacturerBLL(IMapper mapper,
        IReadonlyRepository<Manufacturer> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<SoftwareManufacturerDetails[]> GetListAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.ToArrayAsync(c => c.IsSoftware, cancellationToken);

        return _mapper.Map<SoftwareManufacturerDetails[]>(result);
    }
}
