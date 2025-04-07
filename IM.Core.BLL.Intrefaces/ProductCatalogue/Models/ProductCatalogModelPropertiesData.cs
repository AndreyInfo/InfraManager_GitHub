namespace InfraManager.BLL.ProductCatalogue.Models;
public class ProductCatalogModelPropertiesData
{
    #region Общие свойства
    public bool CanBuy { get; init; }
    #endregion

    #region Свойства для модели адаптера
    public int? SlotTypeID { get; init; }
    public string Parameters { get; init; }
    #endregion

    #region Свойства для модели сетевого оборудования
    public decimal? Height { get; init; }
    public decimal? HeightInUnits { get; init; }
    public decimal? Width { get; init; }
    public decimal? Depth { get; init; }
    public bool? IsRackMount { get; init; }
    #endregion

    #region Свойства для модели оконечного оборудования
    public int? TechnologyTypeID { get; init; }
    public int? ConnectorTypeID { get; init; }
    #endregion
}
