using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ProductCatalogue.Classes;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.ProductTemplates;

internal class ProductCatalogTemplateBLL :
    IProductCatalogTemplateBLL
    , ISelfRegisteredService<IProductCatalogTemplateBLL>
{
    private readonly IMapper _mapper;
    private readonly IReadonlyRepository<ProductCatalogTemplate> _productCatalogTemplates;
    private readonly IGetEntityBLL<ProductTemplate, ProductCatalogTemplate, ProductTemplateInfo> _detailsBLL;
    public ProductCatalogTemplateBLL(IMapper mapper
        , IReadonlyRepository<ProductCatalogTemplate> templatesRepository
        , IGetEntityBLL<ProductTemplate, ProductCatalogTemplate, ProductTemplateInfo> detailsBLL)
    {
        _mapper = mapper;
        _productCatalogTemplates = templatesRepository;
        _detailsBLL = detailsBLL;
    }
    public async Task<HashSet<ProductTemplate>> GetSubTemplatesAsync(IEnumerable<ProductTemplate> templates, CancellationToken cancellationToken)
    {
        var list = new List<ProductCatalogTemplate>();
        foreach (var templateID in templates)
        {
            var template = await _productCatalogTemplates.WithMany(c=> c.ProductCatalogTemplates)
                .DisableTrackingForQuery()
                .FirstOrDefaultAsync(c=> c.ID == templateID, cancellationToken);

            list.AddRange(GetSubTemplates(template));
        }

        return new HashSet<ProductTemplate>(list.Select(c=> c.ID));
    }

    private ProductCatalogTemplate[] GetSubTemplates(ProductCatalogTemplate root)
    {
        var result = new Queue<ProductCatalogTemplate>();

        var templates = new Queue<ProductCatalogTemplate>();
        do
        {
            result.Enqueue(root);
            foreach (var template in root.ProductCatalogTemplates)
                templates.Enqueue(template);
        } while (templates.TryDequeue(out root));

        return result.ToArray();
    }

    public async Task<ProductTemplateInfo[]> GetNodesAsync(ProductTemplateTreeFilter filter, CancellationToken cancellationToken)
    {
        var templates = await _productCatalogTemplates.DisableTrackingForQuery()
                                    .ToArrayAsync(c=> c.ParentID == filter.ParentID, cancellationToken);

        return _mapper.Map<ProductTemplateInfo[]>(templates);
    }

    public async Task<ProductTemplateInfo> GetByID(ProductTemplate id, CancellationToken cancellationToken)
        => await _detailsBLL.DetailsAsync(id, cancellationToken);
    
}