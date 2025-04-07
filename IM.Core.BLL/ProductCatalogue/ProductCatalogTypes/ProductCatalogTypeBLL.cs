using Inframanager;
using Inframanager.BLL;
using Inframanager.DAL.ProductCatalogue.Import;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Localization;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.DAL;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Asset.DeviceMonitors;
using InfraManager.DAL.ConfigurationData;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.ProductCatalogTypes;
using InfraManager.DAL.ProductCatalogue.Tree;
using InfraManager.DAL.Software;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

internal sealed class ProductCatalogTypeBLL :
    StandardBLL<Guid, ProductCatalogType, ProductCatalogTypeData, ProductCatalogTypeDetails, ProductCatalogTypeFilter>
    , IProductCatalogTypeBLL
    , ISelfRegisteredService<IProductCatalogTypeBLL>
{
    private readonly ILocalizeText _localizeText;
    private readonly IProductCatalogTypeNodeQuery _catalogType; 
    private readonly IProductCatalogModelBLLFacade _modelFacade;
    private readonly IValidatePermissions<ProductCatalogType> _permissionValidator;
    private readonly ILoadEntity<Guid, ProductCatalogType, ProductCatalogTypeDetails> _entityLoader;
    private readonly IBuildObject<ProductCatalogTypeDetails, ProductCatalogType> _detailsBuilder;
    private readonly IFinder<ProductCatalogType> _finder;
    private readonly IModifyObject<ProductCatalogType, ProductCatalogTypeData> _entityModifier;
    private readonly IValidateObject<ProductCatalogType> _entityValidator;
    private readonly IUserAccessBLL _userAccess;
    private readonly IAccessPermissionObjectForUserQuery _accessPermissionObjectForUserQuery;

    private readonly IReadonlyRepository<DeviceMonitor> _deviceMonitors;
    private readonly IReadonlyRepository<DataEntity> _dataEntities;
    private readonly IReadonlyRepository<DeviceMonitorParameterTemplate> _deviceMonitorParameterTemplates;
    private readonly IReadonlyRepository<AllowedTypeForLabelPrinting> _allowedTypeForLabelPrintings;
    private readonly IReadonlyRepository<ServiceContract> _serviceContracts;
    private readonly IReadonlyRepository<ServiceContractModel> _serviceContractModels;
    private readonly IReadonlyRepository<ProductCatalogImportSettingTypes> _productCatalogImportSettingTypes;
    private readonly IReadonlyRepository<SoftwareLicence> _softwareLicences;
    public ProductCatalogTypeBLL(IRepository<ProductCatalogType> repository
        , ILogger<ProductCatalogTypeBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<ProductCatalogTypeDetails, ProductCatalogType> detailsBuilder
        , IInsertEntityBLL<ProductCatalogType, ProductCatalogTypeData> insertEntityBLL
        , IModifyEntityBLL<Guid, ProductCatalogType, ProductCatalogTypeData, ProductCatalogTypeDetails> modifyEntityBLL
        , IFinder<ProductCatalogType> finder
        , IModifyObject<ProductCatalogType, ProductCatalogTypeData> entityModifier
        , IValidateObject<ProductCatalogType> entityValidator
        , IRemoveEntityBLL<Guid, ProductCatalogType> removeEntityBLL
        , IGetEntityBLL<Guid, ProductCatalogType, ProductCatalogTypeDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, ProductCatalogType, ProductCatalogTypeDetails, ProductCatalogTypeFilter> detailsArrayBLL
        , ILocalizeText localizeText
        , IProductCatalogTypeNodeQuery catalogType
        , IProductCatalogModelBLLFacade modelFacade
        , IValidatePermissions<ProductCatalogType> permissionValidator
        , ILoadEntity<Guid, ProductCatalogType, ProductCatalogTypeDetails> entityLoader
        , IUserAccessBLL userAccess
        , IAccessPermissionObjectForUserQuery accessPermissionObjectForUserQuery
        , IReadonlyRepository<DeviceMonitor> deviceMonitors
        , IReadonlyRepository<DataEntity> dataEntities
        , IReadonlyRepository<DeviceMonitorParameterTemplate> deviceMonitorParameterTemplates
        , IReadonlyRepository<AllowedTypeForLabelPrinting> allowedTypeForLabelPrintings
        , IReadonlyRepository<ServiceContract> serviceContracts
        , IReadonlyRepository<ServiceContractModel> serviceContractModels
        , IReadonlyRepository<ProductCatalogImportSettingTypes> productCatalogImportSettingTypes
        , IReadonlyRepository<SoftwareLicence> softwareLicences)
        : base(repository
            , logger
            , unitOfWork
            , currentUser
            , detailsBuilder
            , insertEntityBLL
            , modifyEntityBLL
            , removeEntityBLL
            , detailsBLL
            , detailsArrayBLL)
    {
        _localizeText = localizeText; 
        _catalogType = catalogType;
        _modelFacade = modelFacade;
        _permissionValidator = permissionValidator;
        _entityLoader = entityLoader;
        _finder = finder;
        _entityModifier = entityModifier;
        _entityValidator = entityValidator;
        _detailsBuilder = detailsBuilder;
        _userAccess = userAccess;
        _accessPermissionObjectForUserQuery = accessPermissionObjectForUserQuery;
        _deviceMonitors = deviceMonitors;
        _dataEntities = dataEntities;
        _deviceMonitorParameterTemplates = deviceMonitorParameterTemplates;
        _allowedTypeForLabelPrintings = allowedTypeForLabelPrintings;
        _serviceContracts = serviceContracts;
        _serviceContractModels = serviceContractModels;
        _productCatalogImportSettingTypes = productCatalogImportSettingTypes;
        _softwareLicences = softwareLicences;
    }

    public async Task<ProductCatalogNode[]> GetTreeNodesAsync(ProductCatalogTreeFilter filter
        , CancellationToken cancellationToken = default)
    {
        var query = await _catalogType.ExecuteAsync(filter, cancellationToken);

        var isAdmin = await _userAccess.HasAdminRoleAsync(CurrentUser.UserId, cancellationToken);
        if (filter.ByAccess && !isAdmin)
        {
            var accessObjects = await _accessPermissionObjectForUserQuery.ExecuteAsync(CurrentUser.UserId
                , ObjectClass.ProductCatalogType
                , cancellationToken);

            if (accessObjects.Any())
                query = query.Where(x => accessObjects.Select(c => c.ObjectID).Contains(x.ID)).ToArray();
        }

        return query;
    }


    public async Task DeleteAsync(Guid id, ProductCatalogDeleteFlags flags, CancellationToken cancellationToken)
    {
        var filter = new ProductCatalogModelDeleteFilter() { TypeID = id };

        if (flags.IsNoUse)
            await DeleteIfNoUseElseThrowAsync(id, cancellationToken);
        else 
            await DeleteWithModels(filter, flags.WithObjects, cancellationToken);
    }

    private async Task DeleteIfNoUseElseThrowAsync(Guid id, CancellationToken cancellationToken)
    {
        var isUse = await IsUseAsync(id, cancellationToken);
        if (isUse)
            throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.ProductCatalogTypeIsUseInSystem), cancellationToken));

        await DeleteAsync(id, cancellationToken);
    }

    private async Task DeleteWithModels(ProductCatalogModelDeleteFilter filter, bool withObjects, CancellationToken cancellationToken)
    {
        using var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required);
        
        await _modelFacade.DeleteModelsByFilterAsync(filter, withObjects, cancellationToken);
        await DeleteAsync(filter.TypeID.Value, cancellationToken);
        await UnitOfWork.SaveAsync(cancellationToken);
        
        transaction.Complete();
    }

    public async Task<bool> IsUseAsync(Guid typeID, CancellationToken cancellationToken)
    {
        var IsUseModel = await _modelFacade.IsUseTypeAsync(new ProductCatalogModelDeleteFilter() { TypeID = typeID }, cancellationToken);

        return IsUseModel
               || await _deviceMonitors.AnyAsync(c => c.ObjectID == typeID, cancellationToken)
               || await _deviceMonitorParameterTemplates.AnyAsync(c => c.ObjectID == typeID, cancellationToken)
               || await _dataEntities.AnyAsync(c => c.ProductCatalogTypeID == typeID, cancellationToken)
               || await _allowedTypeForLabelPrintings.AnyAsync(c => c.ObjectID == typeID, cancellationToken)
               || await _serviceContracts.AnyAsync(c => c.ProductCatalogTypeID == typeID, cancellationToken)
               || await _serviceContractModels.AnyAsync(c => c.ProductCatalogTypeID == typeID, cancellationToken)
               || await _productCatalogImportSettingTypes.AnyAsync(c => c.ProductCatalogTypeID == typeID, cancellationToken)
               || await _softwareLicences.AnyAsync(c => c.ProductCatalogTypeID == typeID, cancellationToken);
    }

    // TODO: Ниже копипаста методов из StandardBLL, но без проверки прав пользователя.
    // В будущем стоит подумать над лучшим решением, т.к. сущность типа каталога продуктов
    // используется как в SD, так и в админ.консоли.
    public async Task<ProductCatalogTypeDetails> DetailsWithoutTTZAsync(Guid id, CancellationToken cancellationToken)
    {
        Logger.LogTrace($"User (ID = {CurrentUser.UserId}) requested details {typeof(ProductCatalogType).Name} (id = {id}).");
        if (!await _permissionValidator.UserHasPermissionAsync(CurrentUser.UserId, ObjectAction.ViewDetails, cancellationToken))
        {
            throw new AccessDeniedException($"{typeof(ProductCatalogType).Name} view.");
        }
        Logger.LogTrace($"Permissions to view {typeof(ProductCatalogType).Name} is granted to user (ID = {CurrentUser.UserId})");

        var entity = await _entityLoader.LoadAsync(id, cancellationToken);

        Logger.LogTrace($"{typeof(ProductCatalogType)} (id = {id}) is found in database.");

        return await _detailsBuilder.BuildAsync(entity, cancellationToken);
    }

    public async Task<ProductCatalogTypeDetails> UpdateWithoutTTZAsync(Guid id, ProductCatalogTypeData data, CancellationToken cancellationToken = default)
    {
        Logger.LogInformation($"User (ID = {CurrentUser.UserId}) is modifying {typeof(ProductCatalogType).Name} (id = {id}).");
        Logger.TraceObject($"Update to {typeof(ProductCatalogType).Name} (id = {id}) from user (ID = {CurrentUser.UserId})", data);
        if (!await _permissionValidator.UserHasPermissionAsync(CurrentUser.UserId, ObjectAction.Update, cancellationToken))
        {
            throw new AccessDeniedException($"{typeof(ProductCatalogType).Name} update");
        }
        Logger.LogTrace($"Permissions to update {typeof(ProductCatalogType).Name} is granted to user (ID = {CurrentUser.UserId}).");

        var entity = await _entityLoader.LoadAsync(id, cancellationToken);
        Logger.LogTrace($"{typeof(ProductCatalogType)} (id = {id}) is found in database.");

        await _entityModifier.ModifyAsync(entity, data, cancellationToken);

        if (!UnitOfWork.HasChanges())
        {
            throw new NotModifiedException();
        }

        _entityModifier.SetModifiedDate(entity);
        await _entityValidator.ValidateOrRaiseErrorAsync(entity, cancellationToken);
        Logger.LogTrace($"{typeof(ProductCatalogType)} (id = {id}) appears to be valid after being modified by user (ID = {CurrentUser.UserId})");

        await UnitOfWork.SaveAsync(cancellationToken);
        Logger.LogInformation($"User (ID = {CurrentUser.UserId}) successfully updated {typeof(ProductCatalogType).Name} (id = {id})");
        return await _detailsBuilder.BuildAsync(entity, cancellationToken);
    }

    public async Task DeleteWithoutByFlagsTTZAsync(Guid id, ProductCatalogDeleteFlags flags, CancellationToken cancellationToken)
    {
        var filter = new ProductCatalogModelDeleteFilter() { TypeID = id };

        if (flags.IsNoUse)
        {
            var isUse = await IsUseAsync(id, cancellationToken);
            if (isUse)
                throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.ProductCatalogTypeIsUseInSystem), cancellationToken));

            await DeleteWithoutTTZAsync(id, cancellationToken);
        }
        else
        {
            using var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required);

            await _modelFacade.DeleteModelsByFilterAsync(filter, flags.WithObjects, cancellationToken);
            await DeleteWithoutTTZAsync(filter.TypeID.Value, cancellationToken);
            await UnitOfWork.SaveAsync(cancellationToken);

            transaction.Complete();
        }
    }
    private async Task DeleteWithoutTTZAsync(Guid id, CancellationToken cancellationToken)
    {
        Logger.LogInformation($"User (ID = {CurrentUser.UserId}) is deleting {typeof(ProductCatalogType).Name}.");
        if (!await _permissionValidator.UserHasPermissionAsync(CurrentUser.UserId, ObjectAction.Delete, cancellationToken))
        {
            throw new AccessDeniedException($"{typeof(ProductCatalogType).Name} delete");
        }
        Logger.LogTrace($"Permissions to delete {typeof(ProductCatalogType).Name} is granted to user (ID = {CurrentUser.UserId}).");

        var entity = await _finder.FindOrRaiseErrorAsync(id, cancellationToken);
        Logger.LogTrace($"{typeof(ProductCatalogType)} (id = {id}) is found in database.");

        Repository.Delete(entity);

        await UnitOfWork.SaveAsync(cancellationToken);
        Logger.LogInformation($"User (ID = {CurrentUser.UserId}) deleted {typeof(ProductCatalogType).Name} (id = {id}).");
    }
}