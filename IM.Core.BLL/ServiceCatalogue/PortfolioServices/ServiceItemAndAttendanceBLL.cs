using System;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using Microsoft.Extensions.Logging;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using Inframanager;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

internal class ServiceItemAndAttendanceBLL<TEntity, TDetails, TData, TTable> : IServiceItemAndAttendanceBLL<TEntity, TDetails, TData, TTable>
    where TEntity : PortfolioServiceItemAbstract, new()
    where TDetails : PortfolioServcieItemDetails
    where TData : PortfolioServcieItemData
    where TTable : class
{
    private readonly IMapper _mapper;
    private readonly IRepository<TEntity> _enitityRepository;
    private readonly IUnitOfWork _saveChangesCommand;
    private readonly IPortfolioServiceItemBLL<TDetails, TData> _portfolioServiceItemBLL;
    private readonly IGuidePaggingFacade<TEntity, TTable> _guidePaggingFacade;
    private readonly ILogger<ServiceItemAndAttendanceBLL<TEntity, TDetails, TData, TTable>> _logger;
    private readonly ICurrentUser _currentUser;
    private readonly IValidatePermissions<TEntity> _validatePermissions;
    public ServiceItemAndAttendanceBLL(
        IRepository<TEntity> repository,
        IMapper mapper,
        IUnitOfWork saveChangesCommand,
        IPortfolioServiceItemBLL<TDetails, TData> portfolioServiceItemBLL,
        IGuidePaggingFacade<TEntity, TTable> guidePaggingFacade,
        ILogger<ServiceItemAndAttendanceBLL<TEntity, TDetails, TData, TTable>> logger,
        ICurrentUser currentUser,
        IValidatePermissions<TEntity> validatePermissions)
    {
        _mapper = mapper;
        _enitityRepository = repository;
        _saveChangesCommand = saveChangesCommand;
        _portfolioServiceItemBLL = portfolioServiceItemBLL;
        _guidePaggingFacade = guidePaggingFacade;
        _logger = logger;
        _currentUser = currentUser;
        _validatePermissions = validatePermissions;
    }

    public async Task<TDetails[]> GetByServiceIdAsync(Guid serviceID, BaseFilter filter, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var query = _enitityRepository.Query().Where(c => c.ServiceID == serviceID);
        var enitites = await _guidePaggingFacade.GetPaggingAsync(filter,
            query,
            c => c.Name.Contains(filter.SearchString),
            cancellationToken);

        return _mapper.Map<TDetails[]>(enitites);
    }


    public async Task<TDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

        var serviceItem = await _enitityRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, $"Not found {typeof(TEntity).Name}");

        var result = _mapper.Map<TDetails>(serviceItem);
        await _portfolioServiceItemBLL.InitializateSupportLineAndTagsAsync(result, cancellationToken);

        return result;
    }

    public async Task<Guid> AddAsync(TData model, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Insert, cancellationToken);

        using (var transaction =
                 new TransactionScope(
                     TransactionScopeOption.Required,
                     new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                     TransactionScopeAsyncFlowOption.Enabled))
        {
            var saveModel = _mapper.Map<TEntity>(model);
            _enitityRepository.Insert(saveModel);

            await _portfolioServiceItemBLL.SaveSupportLinAndTagsAsync(saveModel.ID, model, cancellationToken);

            await _saveChangesCommand.SaveAsync(cancellationToken);
            transaction.Complete();

            return saveModel.ID;
        }
    }

    public async Task<Guid> UpdateAsync(Guid id, TData model, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancellationToken);

        using (var transaction =
               new TransactionScope(
                   TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled))
        {
            var saveModel = await _enitityRepository.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, "Не найден элемент или услуга сервиса");

            _mapper.Map(model, saveModel);
            await _portfolioServiceItemBLL.SaveSupportLinAndTagsAsync(id, model, cancellationToken);
            await _saveChangesCommand.SaveAsync(cancellationToken);
            transaction.Complete();
            return saveModel.ID;
        }
    }

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

        var entity = await _enitityRepository.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, $"Not found {typeof(TEntity).Name}");

        _enitityRepository.Delete(entity);
        //TODO удаление доп сущностей, не свзяанных по FK

        await _saveChangesCommand.SaveAsync(cancellationToken);
        _logger.LogTrace($"UserID = {_currentUser.UserId} deleted {typeof(TEntity).Name}");
    }
}