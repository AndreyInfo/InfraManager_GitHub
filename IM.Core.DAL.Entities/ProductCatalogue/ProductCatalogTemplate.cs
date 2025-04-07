using System.Collections.Generic;

namespace InfraManager.DAL.ProductCatalogue;

/// <summary>
/// Элемент реестра классов в каталоге продуктов
/// В интерфейсе 6.7 и 7.0 назван как класс
/// Исторически так сложилось
/// </summary>
public class ProductCatalogTemplate
{
    public ProductTemplate ID { get; init; }
    public string Name { get; init; }
    public ProductTemplate? ParentID { get; init; }
    public ObjectClass ClassID { get; init; }

    public virtual ProductCatalogTemplate ParentTemplate { get; init; }
    public virtual IEnumerable<ProductCatalogTemplate> ProductCatalogTemplates { get; init; }
    public virtual ICollection<ProductCatalogType> ProductCatalogTypes { get; set; }
}
