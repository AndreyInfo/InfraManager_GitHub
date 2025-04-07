using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
public class PeripheralTypeAdditionalFields : IProductCatalogModelProperties
{
    public PeripheralTypeAdditionalFields(PeripheralType peripheralType)
    {
        CanBuy = peripheralType.CanBuy;
        Parameters = peripheralType.Parameters;
    }
    public bool CanBuy { get; init; }
    public string Parameters { get; init; }
}