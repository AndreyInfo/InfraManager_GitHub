using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.BLL.Configuration.Technologies;
using InfraManager.BLL.Technologies;
using InfraManager.Core.Extensions;
using InfraManager.DAL;
using InfraManager.DAL.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL.AccessManagement;

namespace InfraManager.BLL.Configuration.Technology;
internal class TechnologyTypeCompatibilityBLL :
    ITechnologyTypeCompatibilityBLL
    , ISelfRegisteredService<ITechnologyTypeCompatibilityBLL>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IReadonlyRepository<TechnologyType> _types;
    private readonly ILogger<TechnologyTypeCompatibilityBLL> _logger;
    private readonly IRepository<TechnologyCompatibilityNode> _compatibilityRepository;
    private readonly IValidatePermissions<TechnologyCompatibilityNode> _validatePermissions;
    private readonly IGuidePaggingFacade<TechnologyType, TechnologyTypeColumns> _guidePaggingFacade;

    public TechnologyTypeCompatibilityBLL(IMapper mapper
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IReadonlyRepository<TechnologyType> types
        , ILogger<TechnologyTypeCompatibilityBLL> logger
        , IRepository<TechnologyCompatibilityNode> compatibilityRepository
        , IValidatePermissions<TechnologyCompatibilityNode> validatePermissions
        , IGuidePaggingFacade<TechnologyType, TechnologyTypeColumns> guidePaggingFacade)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _types = types;
        _logger = logger;
        _validatePermissions = validatePermissions;
        _compatibilityRepository = compatibilityRepository;
        _guidePaggingFacade = guidePaggingFacade;
    }

    public async Task<TechnologyTypeDetails[]> GetListCompatibilityTechTypeByIDAsync(TechnologyTypesFilter filter, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);


        var query = _types.Query().Where(c => c.TechnologyCompatibilityTo.Select(c => c.TechIDFrom).Contains(filter.FromID.Value));

        var entities = await _guidePaggingFacade.GetPaggingAsync(filter,
            query,
            c => c.Name.ToLower().Contains(filter.SearchString.ToLower()),
            cancellationToken);

        return _mapper.Map<TechnologyTypeDetails[]>(entities);
    }

    public async Task RemoveAsync(int fromID, IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

        var nodes = await _compatibilityRepository.ToArrayAsync(c=> c.TechIDFrom == fromID && ids.Contains(c.TechIDTo), cancellationToken);
        nodes.ForEach(c=> _compatibilityRepository.Delete(c));
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task SaveAsync(int fromTechTypeID, IEnumerable<int> toTechTypeID, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);

        foreach (var id in toTechTypeID)
        {
            var isExists = await _compatibilityRepository.AnyAsync(p => p.TechIDFrom == fromTechTypeID && p.TechIDTo == id, cancellationToken);
            if (isExists)
                continue;

            var node = new TechnologyCompatibilityNode(fromTechTypeID, id);
            _compatibilityRepository.Insert(node);
        }

        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task<TechnologyTypeDetails[]> GetListNotCompatibilityTechTypeByIDAsync(TechnologyTypesFilter filter, CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var compatibilityQuery = _compatibilityRepository.Query();

        if (filter.FromID.HasValue)
            compatibilityQuery = compatibilityQuery.Where(c => c.TechIDFrom == filter.FromID);

        var techToIDs = (await compatibilityQuery.ExecuteAsync(cancellationToken)).Select(c => c.TechIDTo);

        var query = _types.Query().Where(c => !techToIDs.Contains(c.ID));
        if (!string.IsNullOrEmpty(filter.SearchString))
            query = query.Where(c => c.Name.ToLower().Contains(filter.SearchString.ToLower()));

        var techTypes = await query.ExecuteAsync(cancellationToken);

        return _mapper.Map<TechnologyTypeDetails[]>(techTypes);
    }
}
