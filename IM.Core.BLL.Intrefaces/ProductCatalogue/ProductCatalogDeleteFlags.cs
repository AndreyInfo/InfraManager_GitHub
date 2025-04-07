namespace InfraManager.BLL.ProductCatalogue;

public class ProductCatalogDeleteFlags
{

    /// <summary>
    /// Удалить если не используется
    /// </summary>
    public bool IsNoUse { get; init; }

    /// <summary>
    /// Удалить с категориями, типами и объектами
    /// </summary>
    public bool WithObjects { get; init; }
}
