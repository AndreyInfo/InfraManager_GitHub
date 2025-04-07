using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
public class CabinetTypeAdditionalFields : IProductCatalogModelProperties
{
    public CabinetTypeAdditionalFields(CabinetType cabinetType)
    {
        VerticalSize = cabinetType.VerticalSize;
        DepthSize = cabinetType.DepthSize;
        ProductNumberCyrillic = cabinetType.ProductNumberCyrillic;
        WidthI = cabinetType.WidthI;
        Height = cabinetType.Height;
        Width = cabinetType.Width;
        NumberingScheme = cabinetType.NumberingScheme;
    }

    public int VerticalSize { get; init; }
    public decimal DepthSize { get; init; }
    public string ProductNumberCyrillic { get; init; }
    public decimal WidthI { get; init; }
    public decimal? Height { get; init; }
    public decimal? Width { get; init; }
    public short NumberingScheme { get; init; }
}
