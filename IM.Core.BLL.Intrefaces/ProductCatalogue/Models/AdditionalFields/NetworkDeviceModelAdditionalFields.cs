using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
public class NetworkDeviceModelAdditionalFields : IProductCatalogModelProperties
{
    public NetworkDeviceModelAdditionalFields(NetworkDeviceModel networkDeviceModel)
    {
        HeightInUnits = networkDeviceModel.HeightInUnits;
        Height = networkDeviceModel.Height;
        Width = networkDeviceModel.Width;
        Depth = networkDeviceModel.Depth;
        IsRackMount = networkDeviceModel.IsRackmount;
        CanBuy = networkDeviceModel.CanBuy;
    }

    public decimal? Height { get; init; }
    public decimal? HeightInUnits { get; init; }
    public decimal? Width { get; init; }
    public decimal? Depth { get; init; }
    public bool? IsRackMount { get; init; }
    public bool CanBuy { get; init; }
}
