using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using InfraManager.BLL.ServiceDesk.MassIncidents.TechnicalFailureCategories;
using InfraManager.Core.Extensions;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.TechnicalFailuresCategory;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.MassIncidents;

public class TechnicalFailureCategoryBLL : StandardBLL<int, TechnicalFailureCategory, TechnicalFailureCategoryData, TechnicalFailureCategoryDetails, TechnicalFailureCategoryFilter>,
    ITechnicalFailureCategoryBLL,
    ISelfRegisteredService<ITechnicalFailureCategoryBLL>
{
    private readonly ILoadEntity<int, TechnicalFailureCategory, TechnicalFailureCategoryDetails> _loader;
    private readonly IValidatePermissions<ServiceTechnicalFailureCategory> _serviceReferencePermissionsValidator;
    private readonly IFinder<Service> _serviceFinder;
    private readonly IMapper _mapper;

    public TechnicalFailureCategoryBLL(
        IRepository<TechnicalFailureCategory> repository,
        ILogger<TechnicalFailureCategoryBLL> logger,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IBuildObject<TechnicalFailureCategoryDetails, TechnicalFailureCategory> detailsBuilder,
        IInsertEntityBLL<TechnicalFailureCategory, TechnicalFailureCategoryData> insertEntityBLL,
        IModifyEntityBLL<int, TechnicalFailureCategory, TechnicalFailureCategoryData, TechnicalFailureCategoryDetails> modifyEntityBLL,
        IRemoveEntityBLL<int, TechnicalFailureCategory> removeEntityBLL,
        IGetEntityBLL<int, TechnicalFailureCategory, TechnicalFailureCategoryDetails> detailsBLL,
        IGetEntityArrayBLL<int, TechnicalFailureCategory, TechnicalFailureCategoryDetails, TechnicalFailureCategoryFilter> detailsArrayBLL,
        IValidatePermissions<ServiceTechnicalFailureCategory> serviceReferencePermissionsValidator,
        ILoadEntity<int, TechnicalFailureCategory, TechnicalFailureCategoryDetails> loader,
        IFinder<Service> serviceFinder,
        IMapper mapper)
        : base(
            repository, 
            logger, 
            unitOfWork, 
            currentUser,
            detailsBuilder, 
            insertEntityBLL, 
            modifyEntityBLL, 
            removeEntityBLL, 
            detailsBLL, 
            detailsArrayBLL)
    {
        _loader = loader;
        _serviceReferencePermissionsValidator = serviceReferencePermissionsValidator;
        _mapper = mapper;
        _serviceFinder = serviceFinder;
    }

    public async Task<ServiceReferenceDetails> AddServiceReferenceAsync(int id, ServiceReferenceData data, CancellationToken cancellationToken = default)
    {
        var flowID = Guid.NewGuid();
        Logger.LogTrace(
            $"FlowID: {flowID}. User (ID = {CurrentUser.UserId}) is attempting to add technical failure category (ID = {id}) to service (ID = {data.ServiceID}) for processing by group (ID = {data.GroupID})");

        await ValidateOperationPermissionAsync(ObjectAction.Insert, cancellationToken);
        Logger.LogTrace($"FlowID: {flowID}. Access is granted.");

        var technicalFailureCategory = await FindCategoryOrRaiseErrorAsync(id, flowID, cancellationToken);
        var service = await _serviceFinder.FindAsync(data.ServiceID, cancellationToken)
            ?? throw new InvalidObjectException("Service not found");
        Logger.LogTrace($"FlowID: {flowID}. Service is found");
        var reference = new ServiceTechnicalFailureCategory(service, data.GroupID);
        technicalFailureCategory.Services.Add(reference);
        await UnitOfWork.SaveAsync(cancellationToken);
        Logger.LogInformation($"User (ID = {CurrentUser.UserId}) added technical failure category (ID = {id}) to service (ID = {data.ServiceID}) for processing by group (ID = {data.GroupID})");

        return _mapper.Map<ServiceReferenceDetails>(reference);
    }

    public async Task<ServiceReferenceDetails> UpdateServiceReferenceAsync(int id, Guid serviceID, ServiceReferenceUpdatableData data, CancellationToken cancellationToken = default)
    {
        var flowID = Guid.NewGuid();
        Logger.LogTrace(
            $"FlowID: {flowID}. User (ID = {CurrentUser.UserId}) is attempting to set processing group (ID = {data.GroupID}) for technical failure category (ID = {id}) and service (ID = {serviceID}).");

        await ValidateOperationPermissionAsync(ObjectAction.Update, cancellationToken);
        Logger.LogTrace($"FlowID: {flowID}. Access is granted.");

        var technicalFailureCategory = await FindCategoryOrRaiseErrorAsync(id, flowID, cancellationToken);
        var serviceReference = FindReference(technicalFailureCategory, serviceID, flowID);
        if (serviceReference == null)
        {
            var service = await _serviceFinder.FindAsync(serviceID, cancellationToken) ?? throw new InvalidObjectException("Service not found");
            serviceReference = new ServiceTechnicalFailureCategory(service, data.GroupID);
            technicalFailureCategory.Services.Add(serviceReference);
        }

        _mapper.Map(data, serviceReference);
        await UnitOfWork.SaveAsync(cancellationToken);
        Logger.LogInformation(
            $"User (ID = {CurrentUser.UserId}) modified technical failure category (ID = {id}) from service (ID = {serviceID}).");

        return _mapper.Map<ServiceReferenceDetails>(serviceReference);
    }

    public async Task DeleteServiceReferenceAsync(int id, Guid serviceID, CancellationToken cancellationToken = default)
    {
        var flowID = Guid.NewGuid();
        Logger.LogTrace(
            $"FlowID: {flowID}. User (ID = {CurrentUser.UserId}) is attempting to delete technical failure category (ID = {id}) from service (ID = {serviceID}).");

        await ValidateOperationPermissionAsync(ObjectAction.Update, cancellationToken);
        Logger.LogTrace($"FlowID: {flowID}. Access is granted.");

        var technicalFailureCategory = await FindCategoryOrRaiseErrorAsync(id, flowID, cancellationToken);
        var serviceReference = FindReference(technicalFailureCategory, serviceID, flowID);
        if (serviceReference == null)
            throw new ObjectNotFoundException<Guid>(serviceID, ObjectClass.Service);

        technicalFailureCategory.Services.Remove(serviceReference);
        await UnitOfWork.SaveAsync(cancellationToken);
        Logger.LogInformation(
            $"User (ID = {CurrentUser.UserId}) removed technical failure category (ID = {id}) from service (ID = {serviceID}).");
    }

    private async Task ValidateOperationPermissionAsync(ObjectAction action, CancellationToken cancellationToken = default)
    {
        if (!await _serviceReferencePermissionsValidator.UserHasPermissionAsync(CurrentUser.UserId, action, cancellationToken))
        {
            throw new AccessDeniedException($"{action} reference between technical failure category and service");
        }
    }

    private async Task<TechnicalFailureCategory> FindCategoryOrRaiseErrorAsync(int id, Guid flowID, CancellationToken cancellationToken = default)
    {
        var category = await _loader.LoadAsync(id, cancellationToken)
            ?? throw new ObjectNotFoundException<int>(id, ObjectClass.TechnicalFailuresCategoryType);
        Logger.LogTrace($"FlowID: {flowID}. Tech failure category is found.");

        return category;
    }

    private ServiceTechnicalFailureCategory FindReference(TechnicalFailureCategory category, Guid serviceID, Guid flowID)
    {
        var serviceReference = category.Services.FirstOrDefault(x => x.Reference.ID == serviceID);
        Logger.LogTrace($"FlowID: {flowID}. Service is found");

        return serviceReference;
    }
}