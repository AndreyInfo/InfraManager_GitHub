using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Calendar;
using InfraManager.BLL.CrudWeb;
using Inframanager.BLL;
using Microsoft.Extensions.Logging;
using Inframanager.BLL.AccessManagement;
using Inframanager;

namespace InfraManager.BLL.Calendar.Exclusions;

internal class ExclusionBLL : IExclusionBLL, ISelfRegisteredService<IExclusionBLL>
{
    private readonly IMapper _mapper;
    private readonly IRepository<Exclusion> _repositoryExclusions;
    private readonly IUnitOfWork _saveChangesCommnd;
    private readonly IGuidePaggingFacade<Exclusion, ExclusionForTable> _pagging;
    private readonly IValidatePermissions<Exclusion> _validatePermissions;
    private readonly ILogger<ExclusionBLL> _logger;
    private readonly ICurrentUser _currentUser;
    public ExclusionBLL(IMapper mapper,
                        IRepository<Exclusion> repositoryExclusions,
                        IUnitOfWork saveChangesCommnd,
                        IGuidePaggingFacade<Exclusion, ExclusionForTable> pagging,
                        IValidatePermissions<Exclusion> validatePermissions,
                        ILogger<ExclusionBLL> logger,
                        ICurrentUser currentUser)
    {
        _mapper = mapper;
        _repositoryExclusions = repositoryExclusions;
        _saveChangesCommnd = saveChangesCommnd;
        _pagging = pagging;
        _validatePermissions = validatePermissions;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(ExclusionDetails model, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);

        var saveModel = _mapper.Map<Exclusion>(model);
        _repositoryExclusions.Insert(saveModel);
        await _saveChangesCommnd.SaveAsync(cancellationToken);

        return saveModel.ID;
    }

    public async Task<Guid> UpdateAsync(ExclusionDetails model, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);

        var foundEntity = await _repositoryExclusions.FirstOrDefaultAsync(c => c.ID == model.ID, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(model.ID, ObjectClass.Exclusion);

        foundEntity = _mapper.Map(model, foundEntity);
        await _saveChangesCommnd.SaveAsync(cancellationToken);

        return foundEntity.ID;
    }

    public async Task<ExclusionDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

        var entity = await _repositoryExclusions.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Exclusion);

        return _mapper.Map<ExclusionDetails>(entity);
    }


    // TODO переделать на единичное удаление
    public async Task DeleteAsync(DeleteModel<Guid>[] models, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

        var deleteModels = _mapper.Map<Exclusion[]>(models);

        foreach (var item in deleteModels)
            _repositoryExclusions.Delete(item);

        await _saveChangesCommnd.SaveAsync(cancellationToken);
    }

    public async Task<ExclusionDetails[]> GetByFilterAsync(ExclusionFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);
        var query = _repositoryExclusions.Query();
        if (filter.ExclusionType.HasValue)
            query = query.Where(c => c.Type == filter.ExclusionType);


        var items = await _pagging.GetPaggingAsync(filter,
            query,
            x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
            cancellationToken);

        return _mapper.Map<ExclusionDetails[]>(items);
    }
}

