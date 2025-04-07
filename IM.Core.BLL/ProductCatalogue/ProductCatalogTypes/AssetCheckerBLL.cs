using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ProductCatalogue.ProductTemplates;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.ProductCatalogTypes;

public class AssetCheckerBLL : IAssetCheckerBLL,ISelfRegisteredService<IAssetCheckerBLL>
{
    private readonly IProductCatalogTemplateBLL _productCatalogTemplate;
    private readonly IReadOnlyList<ProductTemplate> _assetClasses = new List<ProductTemplate>() 
    {
        ProductTemplate.Adapter,
        ProductTemplate.Expense,
        ProductTemplate.Network,
        ProductTemplate.Peripherial,
        ProductTemplate.Terminal,
        ProductTemplate.SoftwareLicense,
        ProductTemplate.ITSystem
    };

    public AssetCheckerBLL(IProductCatalogTemplateBLL productCatalogTemplate)
    {
        _productCatalogTemplate = productCatalogTemplate;
    }
    
    public async Task<bool> HasAssetAsync(ProductCatalogType type, CancellationToken cancellationToken)
    {
        var assetProductTemplates = await _productCatalogTemplate.GetSubTemplatesAsync(_assetClasses, cancellationToken);

        return assetProductTemplates.Contains(type.ProductCatalogTemplateID);
    }
}