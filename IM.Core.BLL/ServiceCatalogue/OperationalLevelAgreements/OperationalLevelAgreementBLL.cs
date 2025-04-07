using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Cloners;
using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Settings;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using InfraManager.DAL.ServiceCatalogue.SLA;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ServiceCatalogue.OperationalLevelAgreements;

public class OperationalLevelAgreementBLL : IOperationalLevelAgreementBLL,
    ISelfRegisteredService<IOperationalLevelAgreementBLL>
{
    private readonly ISLAConcludedWithQuery _concludedWithQuery;
    private readonly IInsertEntityBLL<OperationalLevelAgreement, OperationalLevelAgreementData> _insertEntity;
    private readonly IGetEntityBLL<int, OperationalLevelAgreement, OperationalLevelAgreementDetails> _getEntityBLL;
    private readonly IRemoveEntityBLL<int, OperationalLevelAgreement> _removeEntityBLL;
    private readonly IUnitOfWork _saveChanges;
    private readonly IMapper _mapper;
    private readonly IGuidePaggingFacade<OperationalLevelAgreement, OperationLevelAgreementListItem> _paggingFacade;
    private readonly IRepository<OperationalLevelAgreement> _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IValidatePermissions<OperationalLevelAgreement> _validatePermissions;
    private readonly ILogger<OperationalLevelAgreementBLL> _logger;
    private readonly IManyToManyReferenceBLL<OperationalLevelAgreement, int> _manyReference;
    private readonly IFinder<Service> _serviceFinder;
    private readonly IReadonlyRepository<ManyToMany<OperationalLevelAgreement, Service>> _serviceRepository;
    private readonly IOperationalLevelAgreementServiceQuery _query;
    private readonly IUserColumnSettingsBLL _columnBLL;
    private readonly ICloner<OperationalLevelAgreement> _cloner;

    private readonly IModifyEntityBLL<int, OperationalLevelAgreement, OperationalLevelAgreementData,
        OperationalLevelAgreementDetails> _modifyEntityBLL;
    
    private readonly IColumnMapper<OperationalLevelAgreementServiceListItem, OperationLevelAgreementServiceListItem>
        _orderedColumnQueryBuilder;
    
    public OperationalLevelAgreementBLL(ISLAConcludedWithQuery concludedWithQuery,
        IInsertEntityBLL<OperationalLevelAgreement, OperationalLevelAgreementData> insertEntity,
        IUnitOfWork saveChanges,
        IMapper mapper,
        IGetEntityBLL<int, OperationalLevelAgreement, OperationalLevelAgreementDetails> getEntityBLL,
        IModifyEntityBLL<int, OperationalLevelAgreement, OperationalLevelAgreementData,
            OperationalLevelAgreementDetails> modifyEntityBLL,
        IGuidePaggingFacade<OperationalLevelAgreement, OperationLevelAgreementListItem> paggingFacade,
        IRepository<OperationalLevelAgreement> repository,
        ICurrentUser currentUser,
        IValidatePermissions<OperationalLevelAgreement> validatePermissions,
        ILogger<OperationalLevelAgreementBLL> logger,
        IManyToManyReferenceBLL<OperationalLevelAgreement, int> manyReference,
        IFinder<Service> serviceFinder,
        IReadonlyRepository<ManyToMany<OperationalLevelAgreement, Service>> serviceRepository,
        IOperationalLevelAgreementServiceQuery query,
        IColumnMapper<OperationalLevelAgreementServiceListItem, OperationLevelAgreementServiceListItem> orderedColumnQueryBuilder,
        IUserColumnSettingsBLL columnBLL,
        IRemoveEntityBLL<int, OperationalLevelAgreement> removeEntityBLL,
        ICloner<OperationalLevelAgreement> cloner)
    {
        _concludedWithQuery = concludedWithQuery;
        _insertEntity = insertEntity;
        _saveChanges = saveChanges;
        _mapper = mapper;
        _getEntityBLL = getEntityBLL;
        _modifyEntityBLL = modifyEntityBLL;
        _paggingFacade = paggingFacade;
        _repository = repository;
        _currentUser = currentUser;
        _validatePermissions = validatePermissions;
        _logger = logger;
        _manyReference = manyReference;
        _serviceFinder = serviceFinder;
        _serviceRepository = serviceRepository;
        _query = query;
        _orderedColumnQueryBuilder = orderedColumnQueryBuilder;
        _columnBLL = columnBLL;
        _removeEntityBLL = removeEntityBLL;
        _cloner = cloner;
    }

    #region CRUD

    public async Task<OperationalLevelAgreementDetails[]> ListAsync(BaseFilter filter,
        CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId,
            ObjectAction.ViewDetailsArray, cancellationToken);
        
        var query = _repository.DisableTrackingForQuery().With(x => x.TimeZone).With(x => x.CalendarWorkSchedule)
            .Query();

        var result = await _paggingFacade.GetPaggingAsync(
            filter,
            query,
            x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
            cancellationToken);

        return _mapper.Map<OperationalLevelAgreementDetails[]>(result);
    }
    
    public async Task<OperationalLevelAgreementDetails> AddAsync(OperationalLevelAgreementData data,
        CancellationToken cancellationToken = default)
    {
        var result = await _insertEntity.CreateAsync(data, cancellationToken);
        
        await _saveChanges.SaveAsync(cancellationToken);

        return _mapper.Map<OperationalLevelAgreementDetails>(result);
    }

    public async Task<OperationalLevelAgreementDetails> UpdateAsync(int id, OperationalLevelAgreementData data,
        CancellationToken cancellationToken = default)
    {
        var result = await _modifyEntityBLL.ModifyAsync(id, data, cancellationToken);
        
        await _saveChanges.SaveAsync(cancellationToken);

        return _mapper.Map<OperationalLevelAgreementDetails>(result);
    }

    public Task<OperationalLevelAgreementDetails> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return _getEntityBLL.DetailsAsync(id, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _removeEntityBLL.RemoveAsync(id, cancellationToken);
        
        await _saveChanges.SaveAsync(cancellationToken);
    }

    #endregion

    public async Task AddServiceReferenceAsync(int id, Guid serviceID, CancellationToken cancellationToken = default)
    {
        await _manyReference.AddReferenceAsync(
            x => x.Services,
            id,
            async serviceID => await _serviceFinder.FindAsync(serviceID, cancellationToken),
            serviceID,
            cancellationToken);
        
        await _saveChanges.SaveAsync(cancellationToken);
    }

    public async Task<OperationalLevelAgreementServiceDetails[]> GetServiceReferenceAsync(int id, BaseFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = _serviceRepository.With(x => x.Parent).With(x => x.Reference).ThenWith(x => x.Category)
            .DisableTrackingForQuery().Query().Where(x => x.Parent.ID == id);
        
        //TODO вынести в сервис чтобы получать сразу экспрешен 
        var columns = await _columnBLL.GetAsync(_currentUser.UserId, filter.ViewName, cancellationToken);
        var orderColumn = columns.GetSortColumn();
        var orderExpression = _orderedColumnQueryBuilder.MapToRightColumn(orderColumn.PropertyName)[0];
        var isAscending = orderColumn.Ascending;

        var result = await _query.ExecuteAsync(query, filter.CountRecords, filter.StartRecordIndex, isAscending,
            orderExpression, filter.SearchString, cancellationToken);

        return _mapper.Map<OperationalLevelAgreementServiceDetails[]>(result);
    }

    public async Task RemoveServiceReferenceAsync(int id, Guid serviceID, CancellationToken cancellationToken = default)
    {
        await _manyReference.RemoveReferenceAsync(
            id,
            serviceID,
            x => x.Services,
            x => x.ID,
            cancellationToken);
        
        await _saveChanges.SaveAsync(cancellationToken);
    }
    
    public Task<SLAConcludedWithItem[]> GetConcludedWithAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        return _concludedWithQuery.ExecuteAsync(id, cancellationToken);
    }

    public async Task CloneAsync(int id, OperationalLevelAgreementCloneData data,
        CancellationToken cancellationToken = default)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert,
            cancellationToken);

        var ola = await _repository.WithMany(x => x.ConcludedWith)
            .WithMany(x => x.Rules).WithMany(x => x.Services).ThenWith(x => x.Reference).DisableTrackingForQuery()
            .FirstOrDefaultAsync(x => x.ID == id, cancellationToken);

        var clonedOLA = _cloner
            .SetNewValueTo(x => x.ID)
            .SetNewValueTo(x => x.IMObjID)
            .ForEachSetNewValueTo(x => x.ConcludedWith, x => x.ID)
            .SetNewValueTo(x => x.Services)
            .ForEachSetNewValueTo(x => x.Rules, x => x.OperationalLevelAgreementID)
            .ForEachSetNewValueTo(x => x.Rules, x => x.ID)
            .Clone(ola);

        _mapper.Map(data, clonedOLA);
        _repository.Insert(clonedOLA);

        using (var transaction =
               TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
        {
            await _saveChanges.SaveAsync(cancellationToken);

            foreach (var el in ola.Services)
            {
                await _manyReference.AddReferenceAsync(x => x.Services, clonedOLA.ID,
                    async serviceID => await _serviceFinder.FindAsync(serviceID, cancellationToken),
                    el.Reference.ID,
                    cancellationToken);
            }

            await _saveChanges.SaveAsync(cancellationToken);
            transaction.Complete();
        }
    }
}