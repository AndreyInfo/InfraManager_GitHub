using Inframanager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.Tree;
using InfraManager.ResourcesArea;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogCategories;

internal sealed class ProductCatalogCategoryBLL : 
    StandardBLL<Guid, ProductCatalogCategory, ProductCatalogCategoryData, ProductCatalogCategoryDetails, ProductCatalogCategoryFilter>
    , IProductCatalogCategoryBLL
    , ISelfRegisteredService<IProductCatalogCategoryBLL>
{
    private readonly ILocalizeText _localizeText;
    private readonly IProductCatalogModelBLLFacade _modelFacade;
    private readonly IProductCatalogNodeQuery _productCatalogNodeQuery;
    private readonly IProductCatalogTypeBLL _productCatalogTypeBLL;
    public ProductCatalogCategoryBLL(IRepository<ProductCatalogCategory> repository
        , ILogger<ProductCatalogCategoryBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<ProductCatalogCategoryDetails, ProductCatalogCategory> detailsBuilder
        , IInsertEntityBLL<ProductCatalogCategory, ProductCatalogCategoryData> insertEntityBLL
        , IModifyEntityBLL<Guid, ProductCatalogCategory, ProductCatalogCategoryData, ProductCatalogCategoryDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, ProductCatalogCategory> removeEntityBLL
        , IGetEntityBLL<Guid, ProductCatalogCategory, ProductCatalogCategoryDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, ProductCatalogCategory, ProductCatalogCategoryDetails, ProductCatalogCategoryFilter> detailsArrayBLL
        , ILocalizeText localizeText
        , IProductCatalogModelBLLFacade modelFacade
        , IProductCatalogNodeQuery productCatalogNodeQuery
        , IProductCatalogTypeBLL productCatalogTypeBLL)
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
        _modelFacade = modelFacade;
        _productCatalogNodeQuery = productCatalogNodeQuery;
        _productCatalogTypeBLL = productCatalogTypeBLL;
    }
    
    public async Task<ProductCatalogNode[]> GetTreeNodesAsync(ProductCatalogTreeFilter filter, CancellationToken token)
        => await _productCatalogNodeQuery.ExecuteAsync(filter, token);


    public async Task DeleteAsync(Guid id, ProductCatalogDeleteFlags flags, CancellationToken cancellationToken)
    {
        var filter = new ProductCatalogModelDeleteFilter() { CategoryID = id };

        if (flags.IsNoUse)
            await DeleteIfNoUseElseThrowAsync(id, cancellationToken);
        else
            await DeleteWithCategoriesAndTypes(filter, flags.WithObjects, cancellationToken);
    }

    private async Task DeleteIfNoUseElseThrowAsync(Guid id, CancellationToken cancellationToken)
    {
        var isUse = await IsUseSubTypesAsync(id, cancellationToken);
        if (isUse)
            throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.ProductCatalogCategoryIsUseInSystem), cancellationToken));

        await DeleteAsync(id, cancellationToken);
    }

    private async Task<bool> IsUseSubTypesAsync(Guid id, CancellationToken cancellationToken)
    {
        var types = await GetSubTypesAsync(id, cancellationToken);
        foreach(var type in types) 
        {
            var isUse = await _productCatalogTypeBLL.IsUseAsync(type.IMObjID, cancellationToken);
            if(isUse)
                return true;
        }

        return false;
    }

    private async Task<ProductCatalogType[]> GetSubTypesAsync(Guid id, CancellationToken cancellationToken)
    {
         var categories = await GetCategoriesInTreeAsync(id, cancellationToken);

         return categories.SelectMany(c=> c.ProductCatalogTypes).ToArray();
    }

    private async Task<ProductCatalogCategory[]> GetCategoriesInTreeAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = new Queue<ProductCatalogCategory>();
        var root = await Repository.WithMany(c=> c.SubCategories)
            .WithMany(c=> c.ProductCatalogTypes)
            .FirstOrDefaultAsync(c=> c.ID == id, cancellationToken);

        var parents = new Queue<ProductCatalogCategory>();
        do 
        {
            result.Enqueue(root);
            foreach(var sub in root.SubCategories)
                parents.Enqueue(sub);

        } while(parents.TryDequeue(out root));

        return result.ToArray();
    }

    private async Task DeleteWithCategoriesAndTypes(ProductCatalogModelDeleteFilter filter, bool withObjects, CancellationToken cancellationToken)
    {
        using var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required);

        var types = await GetSubTypesAsync(filter.CategoryID.Value, cancellationToken);
        var deleteFlags = new ProductCatalogDeleteFlags { IsNoUse = false , WithObjects = true };
        foreach (var type in types)
            await _productCatalogTypeBLL.DeleteAsync(type.IMObjID, deleteFlags, cancellationToken);

        var categories = await GetCategoriesInTreeAsync(filter.CategoryID.Value, cancellationToken);
        foreach (var category in categories)
            await DeleteAsync(category.ID, cancellationToken);

        await UnitOfWork.SaveAsync(cancellationToken);
        transaction.Complete();
    }
}