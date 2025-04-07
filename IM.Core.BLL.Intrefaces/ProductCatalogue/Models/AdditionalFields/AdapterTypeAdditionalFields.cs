using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
public class AdapterTypeAdditionalFields : IProductCatalogModelProperties
{
    public AdapterTypeAdditionalFields(AdapterType adapterType)
    {
        SlotTypeID = adapterType.SlotTypeID;
        SlotTypeName = adapterType.SlotType?.Name;
        Parameters = adapterType.Parameters;
        CanBuy = adapterType.CanBuy;
    }

    public int? SlotTypeID { get; init; }
    public string SlotTypeName { get; init; }
    public string Parameters { get; init; }
    public bool CanBuy { get; init; }
}
