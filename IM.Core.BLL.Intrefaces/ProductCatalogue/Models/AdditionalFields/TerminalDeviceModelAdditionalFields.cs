using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
public class TerminalDeviceModelAdditionalFields : IProductCatalogModelProperties
{
    public TerminalDeviceModelAdditionalFields(TerminalDeviceModel terminalDeviceModel)
    {
        TechnologyTypeID = terminalDeviceModel.TechnologyTypeID;
        TechnologyTypeName = terminalDeviceModel.TechnologyType?.Name;
        ConnectorTypeID = terminalDeviceModel.ConnectorTypeID;
        ConnectorTypeName = terminalDeviceModel.ConnectorType?.Name;
        CanBuy = terminalDeviceModel.CanBuy;
    }

    public int? TechnologyTypeID { get; init; }
    public string TechnologyTypeName { get; init; }
    public int? ConnectorTypeID { get; init; }
    public string ConnectorTypeName { get; init; }
    public bool CanBuy { get; init; }
}