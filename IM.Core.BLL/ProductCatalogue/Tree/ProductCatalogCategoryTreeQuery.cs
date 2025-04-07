using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue.ProductCatalogCategories;
using InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;
using InfraManager.DAL.ProductCatalogue.ProductCatalogTypes;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.ProductCatalogue.Tree
{
    internal class ProductCatalogCategoryTreeQuery : IProductCatalogTreeQuery, ISelfRegisteredService<IProductCatalogTreeQuery>
    {
        private readonly IProductCatalogCategoryBLL _productCatalogCategoryBll;
        private readonly IProductCatalogTypeBLL _productCatalogTypeBll;
        private readonly IProductCatalogTreeParentBLL _parentBLL;
        private readonly IProductCatalogTypeParentIDQuery _typeParentIDQuery;
        public ProductCatalogCategoryTreeQuery(IProductCatalogCategoryBLL productCatalogCategoryBll, IProductCatalogTypeBLL productCatalogTypeBll, IProductCatalogTreeParentBLL parentBLL, IProductCatalogTypeParentIDQuery typeParentIDQuery)
        {
            _productCatalogCategoryBll = productCatalogCategoryBll;
            _productCatalogTypeBll = productCatalogTypeBll;
            _parentBLL = parentBLL;
            _typeParentIDQuery = typeParentIDQuery;
        }

        public async Task<ProductCatalogNode[]> GetTreeNodesByParentIdAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            var result = new List<ProductCatalogNode>();
            
            var categoryFilter = new ProductCatalogTreeFilter()
            {
                ParentID = id,
                ClassID = ObjectClass.ProductCatalogCategory
            };
            var catalogs = await _productCatalogCategoryBll.GetTreeNodesAsync(categoryFilter, cancellationToken);
            
            result.AddRange(catalogs);

            var typeFitler = new ProductCatalogTreeFilter()
            {
                ParentID = id,
                ClassID = ObjectClass.ProductCatalogType
            };
            var types = await _productCatalogTypeBll.GetTreeNodesAsync(typeFitler, cancellationToken);
            result.AddRange(types);

            return result.ToArray();
        }

        public async Task<ProductCatalogNode[]> GetTreeNodesAsync(ProductCatalogTreeFilter filter,
            IEnumerable<ObjectClass> objectClasses, CancellationToken cancellationToken = default)
        {
            var result = new List<ProductCatalogNode>();

            result.AddRange(await _productCatalogCategoryBll.GetTreeNodesAsync(filter, cancellationToken));
            
            result.AddRange(await _productCatalogTypeBll.GetTreeNodesAsync(filter, cancellationToken));

            return result.ToArray();
        }



        public async Task<TreeParentsDetails[]> GetTreeParentsDetailsAsync(Guid id, ObjectClass classId,
            CancellationToken token)
        {
            var isType = classId == ObjectClass.ProductCatalogType;
            var actualID = isType
                ? await _typeParentIDQuery.ExecuteAsync(id, token)
                : id;

            var resultIDs = await _parentBLL.GetProductCatalogCategoryParentsData(actualID, token);

            if (!isType) return resultIDs.ToArray();

            var typeCount = resultIDs.Count;
            var details = new TreeParentsDetails
            {
                CountFromRoot = typeCount,
                ID = await _typeParentIDQuery.ExecuteAsync(id, token)
            };

            resultIDs.Add(details);

            return resultIDs.ToArray();
        }
    }
}
