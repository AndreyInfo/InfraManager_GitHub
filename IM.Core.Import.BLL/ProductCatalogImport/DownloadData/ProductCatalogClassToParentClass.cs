using IM.Core.Import.BLL.Interface.Import.Models.DownloadData;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.Extensions.Caching.Memory;

namespace IM.Core.Import.BLL.Import.Importer.DownloadData;

public class ProductCatalogClassToParentClass : IProductCatalogClassToParentClass,
    ISelfRegisteredService<IProductCatalogClassToParentClass>
{
    private const string MemoryCacheKey = "ProductCatalogTemplateParentCache";
    private readonly IReadonlyRepository<ProductCatalogTemplate> _templates;
    private readonly IMemoryCache _cache;

    public ProductCatalogClassToParentClass(IReadonlyRepository<ProductCatalogTemplate> templates, 
        IMemoryCache cache)
    {
        _templates = templates;
        _cache = cache;
    }

    public async Task<ObjectClass?> GetBaseObjectClassAsync(ObjectClass objectClass, CancellationToken token)
    {
        if (!_cache.TryGetValue(MemoryCacheKey, out Dictionary<ObjectClass, ObjectClass> parents))
        {
            parents = await GetObjectParents(token);
            _cache.Set(MemoryCacheKey, parents);
        }

        return parents[objectClass];
    }

    private async Task<Dictionary<ObjectClass, ObjectClass>> GetObjectParents(CancellationToken token)
    {
        var result = new Dictionary<ObjectClass, ObjectClass>();

        var templates = await _templates.ToArrayAsync(token);
        var data = templates.ToDictionary(x => x.ID, x=>x.ClassID);
        
        var parents = templates.ToDictionary(x => x.ID, x => x.ParentID);
        var keys = parents.Keys.ToHashSet();
        
        while (keys.Any())
        {
            foreach (var template in keys.ToList())
            {
                var parentID = parents[template];
                if (parentID.HasValue)
                {
                    var grandParent = parents[parentID.Value];
                    if (grandParent.HasValue)
                    {
                        parents[template] = grandParent;
                    }
                    else 
                    {
                        var templateClassID = data[template];
                        var parentClassID = data[parentID.Value];
                        result[templateClassID] =parentClassID;
                        keys.Remove(template);
                    }
                }
                else //корень дерева шаблонов
                {
                    var classId = data[template];
                    result[classId] =  classId;
                    keys.Remove(template);
                }
            }
        }

        return result;
    }
}