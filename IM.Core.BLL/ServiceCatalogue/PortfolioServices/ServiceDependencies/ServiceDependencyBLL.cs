using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Settings;
using InfraManager.DAL.ServiceCatalog;
using Inframanager.BLL;
using Microsoft.Extensions.Logging;
using Inframanager.BLL.AccessManagement;
using Inframanager;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceDependencies;

internal class ServiceDependencyBLL : IServiceDependencyBLL, ISelfRegisteredService<IServiceDependencyBLL>
{
    private readonly IReadonlyRepository<ServiceDependency> _listQuery;
    private readonly IMapper _mapper;
    private readonly IRepository<ServiceDependency> _serviceDependenciesRepository;
    private readonly IUnitOfWork _saveChangesCommand;
    private readonly IServiceDependencyQuery _listServiceByParentIDQuery;
    private readonly IUserColumnSettingsBLL _columnBLL;
    private readonly ICurrentUser _currentUser;
    private readonly IColumnMapper<ServiceModelItem, ServiceDependencyForTable> _columnMapper;
    private readonly IValidatePermissions<Service> _validatePermissions;
    private readonly ILogger<ServiceDependencyBLL> _logger;
    public ServiceDependencyBLL(IReadonlyRepository<ServiceDependency> listQuery
                                , IMapper mapper
                                , IServiceDependencyQuery serviceDependencyDataProvider
                                , IRepository<ServiceDependency> serviceDependenciesRepository
                                , IUnitOfWork saveChangesCommand
                                , IUserColumnSettingsBLL columnBLL
                                , ICurrentUser currentUser
                                , IColumnMapper<ServiceModelItem, ServiceDependencyForTable> columnMapper
                                , IValidatePermissions<Service> validatePermissions
                                , ILogger<ServiceDependencyBLL> logger
        )
    {
        _listQuery = listQuery;
        _mapper = mapper;
        _listServiceByParentIDQuery = serviceDependencyDataProvider;
        _serviceDependenciesRepository = serviceDependenciesRepository;
        _saveChangesCommand = saveChangesCommand;
        _currentUser = currentUser;
        _columnMapper = columnMapper;
        _columnBLL = columnBLL;
        _validatePermissions = validatePermissions;
        _logger = logger;

    }


    public async Task<InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services.ServiceDetails[]> GetTableAsync(BaseFilter filter, Guid? parentId, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

        var columns = await _columnBLL.GetAsync(_currentUser.UserId, filter.ViewName, cancellationToken);
        var orderColumn = columns.GetSortColumn();
        orderColumn.PropertyName = _columnMapper.MapFirst(orderColumn.PropertyName);

        var services = await _listServiceByParentIDQuery.ExecuteQueryServiceDependencyAsync(_mapper.Map<PaggingFilter>(filter), orderColumn, parentId, cancellationToken);

        return _mapper.Map<InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services.ServiceDetails[]>(services);
    }

    //TODO переделать на единичное удаление
    public async Task<Guid[]> DeleteAsync(ServiceDependencyModel[] models, CancellationToken cancellationToken)
    {
        var result = new List<Guid>();

        try
        {
            foreach (var item in models)
            {

                var deleteModel = new ServiceDependency()
                {
                    ParentServiceID = item.ParentId,
                    ChildServiceID = item.ChildId
                };
                _serviceDependenciesRepository.Delete(deleteModel);
            }
        }
        catch
        {
            result.AddRange(models.Select(c => c.ChildId));
        }

        await _saveChangesCommand.SaveAsync(cancellationToken);

        return result.ToArray();
    }


    public async Task<bool> AddAsync(Guid parentId, Guid childId, CancellationToken cancellationToken)
    {
        await CheckExistsServiceDependencyAsync(parentId, childId, cancellationToken);

        var saveModel = new ServiceDependency()
        {
            ParentServiceID = parentId,
            ChildServiceID = childId
        };

        _serviceDependenciesRepository.Insert(saveModel);
        await _saveChangesCommand.SaveAsync(cancellationToken);

        return true;
    }

    private async Task CheckExistsServiceDependencyAsync(Guid parentID, Guid childID, CancellationToken cancellationToken)
    {
        var isExistsServiceDependency = await _listQuery.AnyAsync(c => c.ParentServiceID == parentID && c.ChildServiceID == childID, cancellationToken);
        if (isExistsServiceDependency)
            throw new InvalidObjectException("Зависимость уже существует"); //TODO LOCALE
    }
}
