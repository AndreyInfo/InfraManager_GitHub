using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue.ProductCatalogCategories;
using InfraManager.DAL.ProductCatalogue.Tree;

namespace InfraManager.BLL.ProductCatalogue.Tree
{
    internal class OwnerProductCatalogTreeQuery : IProductCatalogTreeQuery, ISelfRegisteredService<IProductCatalogTreeQuery>
    {
        private readonly IProductCatalogCategoryBLL _productCatalogCategoryBll;

        public OwnerProductCatalogTreeQuery(IProductCatalogCategoryBLL productCatalogCategoryBll)
        {
            _productCatalogCategoryBll = productCatalogCategoryBll;
        }

        public Task<ProductCatalogNode[]> GetTreeNodesByParentIdAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            var catalogFilter = new ProductCatalogTreeFilter()
            {
                ClassID = ObjectClass.Owner,
                ParentID = default
            };
            return _productCatalogCategoryBll.GetTreeNodesAsync(catalogFilter, cancellationToken);
        }

        public Task<ProductCatalogNode[]> GetTreeNodesAsync(ProductCatalogTreeFilter filter,
            IEnumerable<ObjectClass> objectClasses, CancellationToken cancellationToken = default)
        {
            if (!(filter.AllClassIDisAvailable || filter.AvailableClassID.Contains(ObjectClass.ProductCatalogCategory)
                || (filter.ParentID.HasValue && filter.ParentID != Guid.Empty)))
            {
                return Task.FromResult(Array.Empty<ProductCatalogNode>());
            }
            
            return _productCatalogCategoryBll.GetTreeNodesAsync(filter, cancellationToken);
        }

        public Task<TreeParentsDetails[]> GetTreeParentsDetailsAsync(Guid id, ObjectClass classId,
            CancellationToken token)
        {
            var parents = new TreeParentsDetails()
            {
                CountFromRoot = 0,
                ID = id
            };

            var array = new[]
            {
                parents
            };

            return Task.FromResult(array);
        }
    }
}
