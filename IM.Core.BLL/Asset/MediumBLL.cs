using AutoMapper;
using InfraManager.DAL.Asset;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL;
using Inframanager.BLL;
using Microsoft.Extensions.Logging;
using Inframanager.BLL.AccessManagement;
using Inframanager;

namespace InfraManager.BLL.Asset;

public class MediumBLL : IMeduimBLL, ISelfRegisteredService<IMeduimBLL>
{
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<Medium> _readonlyRepository;
    private readonly IValidatePermissions<Medium> _validatePermissions;
    private readonly ILogger<MediumBLL> _logger;
    private readonly ICurrentUser _currentUser;
    public MediumBLL(IMapper mapper
        , IReadonlyRepository<Medium> readonlyRepository
        , IValidatePermissions<Medium> validatePermissions
        , ILogger<MediumBLL> logger
        , ICurrentUser currentUser
        )
    {
        _mapper = mapper;
        _readonlyRepository = readonlyRepository;
        _currentUser = currentUser;
        _logger = logger;
        _validatePermissions = validatePermissions;
    }
    public async Task<MediumDetails[]> GetListAsync(CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var result = await _readonlyRepository.ToArrayAsync(cancellationToken);
        return _mapper.Map<MediumDetails[]>(result);
    }
}
