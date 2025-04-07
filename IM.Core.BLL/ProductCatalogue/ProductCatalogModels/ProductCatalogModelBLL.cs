using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inframanager;
using Inframanager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue.Tree;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogModels;

internal sealed class ProductCatalogModelBLL<TProductModel> :
    StandardBLL<Guid, TProductModel, ProductCatalogModelData, ProductModelOutputDetails, ProductCatalogTreeFilter>,
    IProductCatalogModelBLL
    where TProductModel : class, IProductModel
{
    private readonly ILocalizeText _localizeText;
    private readonly IValidateObject<ProductCatalogTreeFilter> _filterValidator;
    private readonly IValidatePermissions<TProductModel> _permissionValidator;
    private readonly IBuildObject<ProductModelOutputDetails, TProductModel> _detailsBuilder;
    private readonly ILoadEntity<Guid, TProductModel, ProductModelOutputDetails> _entityLoader;
    private readonly IModifyObject<TProductModel, ProductCatalogModelData> _entityModifier;
    private readonly IValidateObject<TProductModel> _entityValidator;
    private readonly IServiceMapper<ObjectClass, IProductObjectBLL> _serviceMapperObjectBLL;
    private readonly IBuildEntityQuery<TProductModel, ProductModelOutputDetails, ProductCatalogTreeFilter> _entityQuery;

    public ProductCatalogModelBLL(IRepository<TProductModel> repository
        , ILogger<ProductCatalogModelBLL<TProductModel>> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , ILocalizeText localizeText
        , IValidateObject<ProductCatalogTreeFilter> filterValidator
        , IValidatePermissions<TProductModel> permissionValidator
        , IBuildObject<ProductModelOutputDetails, TProductModel> detailsBuilder
        , ILoadEntity<Guid, TProductModel, ProductModelOutputDetails> entityLoader
        , IModifyObject<TProductModel, ProductCatalogModelData> entityModifier
        , IValidateObject<TProductModel> entityValidator
        , IInsertEntityBLL<TProductModel, ProductCatalogModelData> insertEntityBLL
        , IModifyEntityBLL<Guid, TProductModel, ProductCatalogModelData, ProductModelOutputDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, TProductModel> removeEntityBLL
        , IGetEntityBLL<Guid, TProductModel, ProductModelOutputDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, TProductModel, ProductModelOutputDetails, ProductCatalogTreeFilter> detailsArrayBLL
        , IBuildEntityQuery<TProductModel, ProductModelOutputDetails, ProductCatalogTreeFilter> entityQuery
        , IServiceMapper<ObjectClass, IProductObjectBLL> serviceMapperObjectBLL)
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
        _filterValidator = filterValidator;
        _permissionValidator = permissionValidator;
        _detailsBuilder = detailsBuilder;
        _entityLoader = entityLoader;
        _entityModifier = entityModifier;
        _entityValidator = entityValidator;
        _entityQuery = entityQuery;
        _serviceMapperObjectBLL = serviceMapperObjectBLL;
    }

    public async Task<ProductModelOutputDetails[]> GetByFilterAsync(ProductCatalogModelFilter filter,
        CancellationToken token)
        => await GetDetailsArrayAsync(filter.GetTreeFilter(), token);


    public async Task DeleteByFilterAsync(ProductCatalogTreeFilter treeFilter, bool withObjects, CancellationToken cancellationToken)
    {
        var models = await _entityQuery.Query(treeFilter).ExecuteAsync(cancellationToken);

        foreach (var model in models)
        {
            Repository.Delete(model);
            if (withObjects)
            {
                await DeleteObjectsByModelID(model.IMObjID, cancellationToken);
            }
        }

        await UnitOfWork.SaveAsync(cancellationToken);
    }

    public async Task DeleteByFlagsAsync(Guid id, ProductCatalogDeleteFlags flags, CancellationToken cancellationToken)
    {
        if (flags.IsNoUse)
            await DeleteIfNoUseElseThrowAsync(id, cancellationToken);
        else
        {
            var model = await Repository.FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken);
            Repository.Delete(model);

            if (flags.WithObjects)
                await DeleteObjectsByModelID(id, cancellationToken);
        }

        await UnitOfWork.SaveAsync(cancellationToken);
    }

    private async Task DeleteIfNoUseElseThrowAsync(Guid id,  CancellationToken cancellationToken)
    {
        var isUse = await IsUseAsync(id, cancellationToken);
        if (isUse)
            throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.ProductCatalogModelIsUseInSystem), cancellationToken));
        
        await DeleteAsync(id, cancellationToken);
    }

    private async Task DeleteObjectsByModelID(Guid modelID, CancellationToken cancellationToken)
    {
        var modelClassID = typeof(TProductModel).GetObjectClassOrRaiseError();
        var objectsClassID = GetObjectsClassByModelClassID(modelClassID);

        foreach (var objectClass in objectsClassID)
            await _serviceMapperObjectBLL.Map(objectClass).DeleteByModelIDAsync(modelID, cancellationToken);
    }

    private async Task<bool> IsUseAsync(Guid id, CancellationToken cancellationToken)
    {
        var modelClassID = typeof(TProductModel).GetObjectClassOrRaiseError();
        var objectClassIDs = GetObjectsClassByModelClassID(modelClassID);

        foreach (var objectClass in objectClassIDs)
            if (await _serviceMapperObjectBLL.Map(objectClass).HasObjectsInModelAsync(id, cancellationToken))
                return true;

        return false;
    }

    //TODO узнать, может есть еще некоторые зависимости меж объектами и моделями, поэтому пока возвращает массив
    public ObjectClass[] GetObjectsClassByModelClassID(ObjectClass productClass)
        => productClass switch
        {
            ObjectClass.TerminalDeviceModel => new ObjectClass[] { ObjectClass.TerminalDevice },
            ObjectClass.AdapterModel=> new ObjectClass[] {ObjectClass.Adapter },
            ObjectClass.PeripherialModel => new ObjectClass[] { ObjectClass.Peripherial },
            ObjectClass.NetworkDeviceModel=> new ObjectClass[] { ObjectClass.ActiveDevice },
            ObjectClass.SoftwareLicenseModel=> new ObjectClass[] { ObjectClass.SoftwareLicence },
            ObjectClass.MaterialModel => new ObjectClass[] { ObjectClass.Material, ObjectClass.MaterialCartridge },
            ObjectClass.CabinetType => new ObjectClass[] { ObjectClass.Rack },
            _ => throw new InvalidObjectException("Некорректный ClassID модели")
        };

    // TODO: Ниже копипаста методов из StandardBLL, но без проверки прав пользователя.
    // В будущем стоит подумать над лучшим решением, т.к. модели каталога продуктов
    // используется как в SD, так и в админ.консоли.
    public async Task<ProductModelOutputDetails[]> GetDetailsArrayWithoutTTZAsync(ProductCatalogTreeFilter filterBy, CancellationToken cancellationToken = default)
    {
        Logger.LogTrace($"User (ID = {CurrentUser.UserId}) requested details of {typeof(ProductModelOutputDetails).Name} filter by {filterBy}.");
        await _filterValidator.ValidateOrRaiseErrorAsync(filterBy, cancellationToken);

        if (!await _permissionValidator.UserHasPermissionAsync(CurrentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken))
        {
            throw new AccessDeniedException($"{typeof(TProductModel).Name} details array.");
        }
        Logger.LogTrace($"Permissions to get {typeof(TProductModel).Name} details array is granted to user (ID = {CurrentUser.UserId}).");

        var query = _entityQuery.Query(filterBy);

        var entities = await query.ExecuteAsync(cancellationToken);
        Logger.LogTrace($"{entities.Count()} elements of type {typeof(TProductModel).Name} is loaded for user (ID = {CurrentUser.UserId})");

        var details = await _detailsBuilder.BuildManyAsync(entities, cancellationToken);

        return details.ToArray();
    }

    public async Task<ProductModelOutputDetails> DetailsWithoutTTZAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Logger.LogTrace($"User (ID = {CurrentUser.UserId}) requested details {typeof(TProductModel).Name} (id = {id}).");
        if (!await _permissionValidator.UserHasPermissionAsync(CurrentUser.UserId, ObjectAction.ViewDetails, cancellationToken))
        {
            throw new AccessDeniedException($"{typeof(TProductModel).Name} view.");
        }
        Logger.LogTrace($"Permissions to view {typeof(TProductModel).Name} is granted to user (ID = {CurrentUser.UserId})");

        var entity = await _entityLoader.LoadAsync(id, cancellationToken);

        Logger.LogTrace($"{typeof(TProductModel)} (id = {id}) is found in database.");

        return await _detailsBuilder.BuildAsync(entity, cancellationToken);
    }

    public async Task<ProductModelOutputDetails> UpdateWithoutTTZAsync(Guid id, ProductCatalogModelData data, CancellationToken cancellationToken = default)
    {
        Logger.LogInformation($"User (ID = {CurrentUser.UserId}) is modifying {typeof(TProductModel).Name} (id = {id}).");
        Logger.TraceObject($"Update to {typeof(TProductModel).Name} (id = {id}) from user (ID = {CurrentUser.UserId})", data);
        if (!await _permissionValidator.UserHasPermissionAsync(CurrentUser.UserId, ObjectAction.Update, cancellationToken))
        {
            throw new AccessDeniedException($"{typeof(TProductModel).Name} update");
        }
        Logger.LogTrace($"Permissions to update {typeof(TProductModel).Name} is granted to user (ID = {CurrentUser.UserId}).");

        var entity = await _entityLoader.LoadAsync(id, cancellationToken);
        Logger.LogTrace($"{typeof(TProductModel)} (id = {id}) is found in database.");

        await _entityModifier.ModifyAsync(entity, data, cancellationToken);

        if (!UnitOfWork.HasChanges())
        {
            throw new NotModifiedException();
        }

        _entityModifier.SetModifiedDate(entity);
        await _entityValidator.ValidateOrRaiseErrorAsync(entity, cancellationToken);
        Logger.LogTrace($"{typeof(TProductModel)} (id = {id}) appears to be valid after being modified by user (ID = {CurrentUser.UserId})");

        await UnitOfWork.SaveAsync(cancellationToken);
        Logger.LogInformation($"User (ID = {CurrentUser.UserId}) successfully updated {typeof(TProductModel).Name} (id = {id})");
        return await _detailsBuilder.BuildAsync(entity, cancellationToken);
    }
}
