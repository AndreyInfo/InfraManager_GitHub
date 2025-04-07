using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Asset;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.ServiceUnits;

internal class ServiceUnitBLL : IServiceUnitBLL, ISelfRegisteredService<IServiceUnitBLL>
{
    private readonly IRepository<ServiceUnit> _repositoryServiceUnits;
    private readonly IRepository<OrganizationItemGroup> _repositoryOrganizationItemGroups;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IServiceUnitPerformersBLL _serviceUnitPerformersBLL;
    
    private readonly IValidatePermissions<ServiceUnit> _validatePermissions;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<ServiceUnit> _logger;

    private Guid _currentUserID => _currentUser.UserId;
    public ServiceUnitBLL(IRepository<ServiceUnit> repositoryServiceUnits
                          , IRepository<OrganizationItemGroup> repositoryOrganizationItemGroups
                          , IUnitOfWork unitOfWork
                          , IMapper mapper
                          , IServiceUnitPerformersBLL serviceUnitPerformersBLL
                          , IValidatePermissions<ServiceUnit> validatePermissions
                          , ICurrentUser currentUser
                          , ILogger<ServiceUnit> logger)
    {
        _repositoryServiceUnits = repositoryServiceUnits;
        _repositoryOrganizationItemGroups = repositoryOrganizationItemGroups;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _serviceUnitPerformersBLL = serviceUnitPerformersBLL;
        _validatePermissions = validatePermissions;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<ServiceUnitDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"UserID = {_currentUserID} requested {nameof(ServiceUnit)} with id = {id}");
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUserID, ObjectAction.ViewDetails, cancellationToken);
        await ThrowIfNotExistsAsync(id, cancellationToken);
        _logger.LogTrace($"UserID = {_currentUserID} has permission to get  {typeof(ServiceUnit).Name}");

        var entity = await _repositoryServiceUnits.With(c => c.ResponsibleUser)
                                                  .FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
                                                  ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.ServiceUnit);

        var result = _mapper.Map<ServiceUnitDetails>(entity);
        result.Performers = await GetPerformersByServiceUnitIDAsync(id, entity.ResponsibleUser, cancellationToken);

        return result;
    }

    private async Task<PerformerDetails[]> GetPerformersByServiceUnitIDAsync(Guid serviceUnitID, User responsibleUser, CancellationToken cancellationToken)
    {
        var result = new List<PerformerDetails>();
        if (responsibleUser is not null)
            result.Add(_mapper.Map<PerformerDetails>(responsibleUser));
        result.AddRange(await _serviceUnitPerformersBLL.GetPerformersByServiceUnitIdAsync(serviceUnitID, cancellationToken));

        return result.ToArray();
    }

    public async Task<ServiceUnitDetails> AddAsync(ServiceUnitInsertDetails serviceUnitDetails, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUserID, ObjectAction.Insert, cancellationToken);
        _logger.LogTrace($"UserID = {_currentUserID} has permission to insert {typeof(ServiceUnit).Name}");

        var entity = _mapper.Map<ServiceUnit>(serviceUnitDetails);
        await ThrowIfExistsSameNameAsync(entity.ID, serviceUnitDetails.Name, cancellationToken);
        _repositoryServiceUnits.Insert(entity);
        
        var performers = _mapper.Map<OrganizationItemGroup[]>(serviceUnitDetails.Performers);
        await UpdatePerformersAsync(entity.ID, entity.ResponsibleID, performers, cancellationToken);
        
        await _unitOfWork.SaveAsync(cancellationToken);
        _logger.LogTrace($"UserID = {_currentUserID} inserted new {typeof(ServiceUnit).Name}");
        return await GetByIDAsync(entity.ID, cancellationToken);
    }

    private async Task UpdatePerformersAsync(Guid serviceUnitID, Guid responsibleID, OrganizationItemGroup[] organizationItemGroups, CancellationToken cancellationToken)
    {
        var organizationItemGroupsForDelete = await _repositoryOrganizationItemGroups.ToArrayAsync(c => c.ID == serviceUnitID, cancellationToken);
        organizationItemGroupsForDelete.ForEach(c => _repositoryOrganizationItemGroups.Delete(c));

        foreach (var item in organizationItemGroups)
        {
            if (responsibleID == item.ItemID)
                continue;

            var newItem = new OrganizationItemGroup(serviceUnitID, item.ItemID, item.ItemClassID);
            _repositoryOrganizationItemGroups.Insert(newItem);
        }
    }

    public async Task<ServiceUnitDetails> UpdateAsync(ServiceUnitDetails serviceUnitDetails, Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUserID, ObjectAction.Update, cancellationToken);
        await ThrowIfNotExistsAsync(id, cancellationToken);
        await ThrowIfExistsSameNameAsync(id, serviceUnitDetails.Name, cancellationToken);

        _logger.LogTrace($"UserID = {_currentUserID} update {typeof(ServiceUnit).Name}");

        var entity = await _repositoryServiceUnits.FirstOrDefaultAsync(c => c.ID == id, cancellationToken);
        _mapper.Map(serviceUnitDetails, entity);
        
        var performers = _mapper.Map<OrganizationItemGroup[]>(serviceUnitDetails.Performers);
        await UpdatePerformersAsync(entity.ID, entity.ResponsibleID, performers, cancellationToken);

        await _unitOfWork.SaveAsync(cancellationToken);
        _logger.LogTrace($"UserID = {_currentUserID} updated {typeof(ServiceUnit).Name}");
        return serviceUnitDetails;
    }

    private async Task ThrowIfNotExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var isExists = await _repositoryServiceUnits.AnyAsync(c => c.ID == id, cancellationToken);
        if (!isExists)
            throw new ObjectNotFoundException<Guid>(id, ObjectClass.ServiceUnit);
    }

    private async Task ThrowIfExistsSameNameAsync(Guid id, string name, CancellationToken cancellationToken)
    {
        var isExists = await _repositoryServiceUnits.AnyAsync(c => c.ID != id && c.Name.Equals(name), cancellationToken);
        if (isExists)
            throw new InvalidObjectException($"Сервисный блок с таким именем {name} уже существует");
    }

}
