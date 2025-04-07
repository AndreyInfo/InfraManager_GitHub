using AutoMapper;
using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Settings;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceCatalog;
using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

internal class ServiceBLL : IServiceBLL, ISelfRegisteredService<IServiceBLL>
{
    private readonly IMapper _mapper;
    private readonly IPortfolioServiceItemBLL<PortfolioServcieItemDetails, PortfolioServcieItemData> _portfolioServiceItemBLL;

    private readonly IReadonlyRepository<User> _userRepository;
    private readonly IReadonlyRepository<Group> _groupQueueModelFinder;
    private readonly IReadonlyRepository<Subdivision> _subdivisionRepository;

    private readonly IReadonlyRepository<ServiceCategory> _serviceCategoryRepository;
    private readonly IRepository<Service> _repositoryService;
    private readonly IUnitOfWork _saveChangesCommand;

    private readonly IListServiceQuery _serviceQuery;
    private readonly IUserColumnSettingsBLL _columnBLL;
    private readonly ICurrentUser _currentUser;
    private readonly IColumnMapper<ServiceModelItem, PortfolioServiceForTable> _columnMapper;
    public ServiceBLL(IMapper mapper
                      , IReadonlyRepository<User> userFinder
                      , IReadonlyRepository<Group> groupQueueModelFinder
                      , IReadonlyRepository<Subdivision> subdivisionQueueModelFinder
                      , IReadonlyRepository<ServiceCategory> serviceCategoryRepository
                      , IRepository<Service> repositoryService
                      , IUnitOfWork saveChangesCommand
                      , IPortfolioServiceItemBLL<PortfolioServcieItemDetails, PortfolioServcieItemData> portfolioServiceItemBLL
                      , IListServiceQuery serviceQuery
                      , IUserColumnSettingsBLL columnBLL
                      , ICurrentUser currentUser
                      , IColumnMapper<ServiceModelItem, PortfolioServiceForTable> columnMapper)
    {
        _mapper = mapper;
        _userRepository = userFinder;
        _groupQueueModelFinder = groupQueueModelFinder;
        _subdivisionRepository = subdivisionQueueModelFinder;
        _repositoryService = repositoryService;
        _saveChangesCommand = saveChangesCommand;
        _portfolioServiceItemBLL = portfolioServiceItemBLL;
        _serviceCategoryRepository = serviceCategoryRepository;
        _serviceQuery = serviceQuery;
        _columnBLL = columnBLL;
        _currentUser = currentUser;
        _columnMapper = columnMapper;
    }


    public async Task<ServiceDetails[]> GetListByCategoryIDAsync(Guid categoryID, CancellationToken cancellationToken = default)
    {
        var isExsitsCatalog = await _serviceCategoryRepository.AnyAsync(c => c.ID == categoryID, cancellationToken);
        if (!isExsitsCatalog)
            throw new ObjectNotFoundException<Guid>(categoryID, ObjectClass.ServiceCategory);

        var services = await _repositoryService.ToArrayAsync(c => c.CategoryID == categoryID, cancellationToken);
        return _mapper.Map<ServiceDetails[]>(services);
    }


    public async Task<PortfolioServiceItemTable[]> GetServicesForTableAsync(ServiceFilter filter, CancellationToken cancellationToken = default)
    {
        var columns = await _columnBLL.GetAsync(_currentUser.UserId, filter.ViewName, cancellationToken);
        var orderColumn = columns.GetSortColumn();
        orderColumn.PropertyName = _columnMapper.MapFirst(orderColumn.PropertyName);

        var entities = await _serviceQuery.ExecuteAsync(_mapper.Map<PaggingFilter>(filter), orderColumn, filter.CategoryID, cancellationToken);

        return _mapper.Map<PortfolioServiceItemTable[]>(entities);
    }


    public async Task<ServiceDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken)
    {
        var serviceEntity = await _repositoryService.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Service);

        var result = _mapper.Map<ServiceDetails>(serviceEntity);
        await _portfolioServiceItemBLL.InitializateSupportLineAndTagsAsync(result, cancellationToken);
        await SetCustomerAsync(result, cancellationToken);

        return result;
    }

    /// <summary>
    /// Инициализация Заказчика
    /// </summary>
    /// <param name="item"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task SetCustomerAsync(ServiceDetails item, CancellationToken cancellationToken)
    {
        if (!item.OrganizationItemObjectID.HasValue)
            return;

        if (item.OrganizationItemClassID == ObjectClass.User)
        {
            var user = await _userRepository.FirstOrDefaultAsync(c => c.IMObjID == item.OrganizationItemObjectID.Value, cancellationToken);
            if (user is null)
                return;
            item.OwnerName = user.FullName;
        }
        else if (item.OrganizationItemClassID == ObjectClass.Group)
        {
            var queue = await _groupQueueModelFinder.FirstOrDefaultAsync(c => c.IMObjID == item.OrganizationItemObjectID.Value, cancellationToken);
            if (queue is null)
                return;
            item.OwnerName = queue.Name;
        }
        else if (item.OrganizationItemClassID == ObjectClass.Division)
        {
            var subdivision = await _subdivisionRepository.FirstOrDefaultAsync(c => c.ID == item.OrganizationItemObjectID.Value, cancellationToken);
            if (subdivision is null)
                return;
            item.OwnerName = subdivision.Name;
        }
    }

    public async Task<Guid> AddAsync(ServiceData model, CancellationToken cancellationToken = default)
    {
        using var transaction =
                 new TransactionScope(
                     TransactionScopeOption.Required,
                     new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                     TransactionScopeAsyncFlowOption.Enabled);

        var saveModel = _mapper.Map<Service>(model);
        _repositoryService.Insert(saveModel);
        await _portfolioServiceItemBLL.SaveSupportLinAndTagsAsync(saveModel.ID, model, cancellationToken);

        await _saveChangesCommand.SaveAsync(cancellationToken);

        transaction.Complete();
        return saveModel.ID;
    }

    public async Task<Guid> UpdateAsync(ServiceData model, Guid id, CancellationToken cancellationToken = default)
    {
        using var transaction =
                 new TransactionScope(
                     TransactionScopeOption.Required,
                     new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                     TransactionScopeAsyncFlowOption.Enabled);

        var foundEntity = await _repositoryService.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.Service);

        foundEntity = _mapper.Map(model, foundEntity);
        await _portfolioServiceItemBLL.SaveSupportLinAndTagsAsync(id, model, cancellationToken);

        await _saveChangesCommand.SaveAsync(cancellationToken);

        transaction.Complete();
        return foundEntity.ID;
    }

    public async Task<ServiceDetailsModel> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _repositoryService.FirstOrDefaultAsync(x => x.ID == id, cancellationToken)
            ?? throw new ObjectNotFoundException($"Service (ID = {id})");

        return _mapper.Map<ServiceDetailsModel>(item);
    }
}
