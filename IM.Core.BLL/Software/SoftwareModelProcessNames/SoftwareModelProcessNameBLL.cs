using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Software;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelProcessNames;

public class SoftwareModelProcessNameBLL : ISoftwareModelProcessNameBLL, ISelfRegisteredService<ISoftwareModelProcessNameBLL>
{
    private readonly ILogger<SoftwareModelProcessNameBLL> _logger;
    private readonly IValidatePermissions<SoftwareModel> _validatePermissions;
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<SoftwareModel> _softwareModelsRepository;
    private readonly IUnitOfWork _saveChangesCommand;
    private readonly IMapper _mapper;


    public SoftwareModelProcessNameBLL(
        ILogger<SoftwareModelProcessNameBLL> logger,
        IValidatePermissions<SoftwareModel> validatePermissions,
        ICurrentUser currentUser,
        IRepository<SoftwareModel> softwareModelsRepository,
        IUnitOfWork saveChangesCommand,
        IMapper mapper
        )
    {
        _logger = logger;
        _validatePermissions = validatePermissions;
        _currentUser = currentUser;
        _softwareModelsRepository = softwareModelsRepository;
        _saveChangesCommand = saveChangesCommand;
        _mapper = mapper;
    }

    public async Task<SoftwareModelProcessNameDetails[]> GetListAsync(SoftwareModelProcessNameFilter filter, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

        var softwareModel = await _softwareModelsRepository
            .FirstOrDefaultAsync(x => x.ID == filter.ID, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(filter.ID, ObjectClass.SoftwareModel);

        var processNames = softwareModel.ProcessNames.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        if (!string.IsNullOrEmpty(filter.SearchString))
        {
            processNames = processNames
                .Where(x => x.ToLower().Contains(filter.SearchString.ToLower()))
                .Skip(filter.StartRecordIndex)
                .Take(filter.CountRecords)
                .ToArray();
        }
        
        if (softwareModel.ProcessNames.Length == 0)
        {
            processNames = Array.Empty<string>();
        }

        return _mapper.Map<SoftwareModelProcessNameDetails[]>(processNames);
    }

    public async Task AddProcessNameToSoftwareModelAsync(SoftwareModelProcessNameData processNameData, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);

        var softwareModel = await _softwareModelsRepository
            .FirstOrDefaultAsync(x => x.ID == processNameData.SoftwareModelID, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(processNameData.SoftwareModelID, ObjectClass.SoftwareModel);

        softwareModel.ProcessNames = string.Join('\n', softwareModel.ProcessNames, processNameData.ProcessNames);

        await _saveChangesCommand.SaveAsync(cancellationToken);
    }

    public async Task DeleteProcessNameFromSoftwareModelAsync(SoftwareModelProcessNameData processNameData, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);

        var softwareModel = await _softwareModelsRepository
            .FirstOrDefaultAsync(x => x.ID == processNameData.SoftwareModelID, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(processNameData.SoftwareModelID, ObjectClass.SoftwareModel);

        var processNames = softwareModel.ProcessNames.Split('\n', StringSplitOptions.None).ToList();

        processNames.Remove(processNameData.ProcessNames);

        softwareModel.ProcessNames = string.Join('\n', processNames);

        await _saveChangesCommand.SaveAsync(cancellationToken);
    }
}
