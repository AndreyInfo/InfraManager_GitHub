using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue.Tree;
using InfraManager.DAL.ProductCatalogue.Tree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.Api.ProductCatalogue
{
    [Authorize]
    [ApiController]
    [Route("api/ProductCatalog")]
    public class ProductCatalogTreeNodesController : ControllerBase
    {

        private readonly IProductCatalogTree _productCatalogTree;
        
        public ProductCatalogTreeNodesController(IProductCatalogTree productCatalogTree)
        {
            _productCatalogTree = productCatalogTree;
        }

        [HttpGet("TreeNodes")]
        public Task<ProductCatalogNode[]> GetProductCatalogTreeNode([FromQuery]Guid? parentId,  CancellationToken token)
        {
            return _productCatalogTree.GetTreeNodeByParentAsync(parentId, token);
        }
    }
}   