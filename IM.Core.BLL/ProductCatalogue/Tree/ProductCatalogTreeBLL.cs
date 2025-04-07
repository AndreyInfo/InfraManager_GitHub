using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ProductCatalogue.Tree;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ProductCatalogue.Tree
{
    internal class ProductCatalogTreeBLL : IProductCatalogTree, ISelfRegisteredService<IProductCatalogTree>
    {
        private readonly IServiceMapper<ObjectClass,IProductCatalogTreeQuery> _treeQueries;
        private readonly IProductCatalogTreeNodeGetTypeQuery _getClass;

        private HashSet<ObjectClass> AvailableClasses => new()
        {
            ObjectClass.SoftwareLicence,
            ObjectClass.IndependentSoftwareLicence,
            ObjectClass.RentSoftwareLicence,
            ObjectClass.UpgradeSoftwareLicence,
            ObjectClass.SubscriptionSoftwareLicence,
            ObjectClass.ExtensionSoftwareLicence,
            ObjectClass.OEMSoftwareLicence
        };


        public ProductCatalogTreeBLL(IServiceMapper<ObjectClass,IProductCatalogTreeQuery> treeQueries
            , IProductCatalogTreeNodeGetTypeQuery getClass)
        {
            _treeQueries = treeQueries;
            _getClass = getClass;
        }


        public async Task<ProductCatalogNode[]> GetTreeNodeByParentAsync(Guid? id, CancellationToken token)
        {
            var actualID = id ?? default;

            ObjectClass nodeClass;
            if (!id.HasValue)
                nodeClass = ObjectClass.Owner;
            else
            {
                nodeClass = await _getClass.ExecuteAsync(actualID, token);
            }

            var bll = GetBll(nodeClass);

            return await bll.GetTreeNodesByParentIdAsync(actualID, token);
        }

        private IProductCatalogTreeQuery GetBll(ObjectClass nodeClass)
        {
            if (!_treeQueries.HasKey(nodeClass))
                return _treeQueries.Map(ObjectClass.Unknown);
            
            var bll = _treeQueries.Map(nodeClass)
                ?? throw new InvalidObjectException(Resources.ProductCatalogTreeClassNotFound);

            return bll;
        }

        public async Task<ProductCatalogNode[]> ExecuteAsync(ProductCatalogTreeFilter filter,
            CancellationToken cancellationToken = default)
        {
            var query = GetBll(filter.ClassID);

            return await query.GetTreeNodesAsync(filter, AvailableClasses, cancellationToken);
        }
    }
}