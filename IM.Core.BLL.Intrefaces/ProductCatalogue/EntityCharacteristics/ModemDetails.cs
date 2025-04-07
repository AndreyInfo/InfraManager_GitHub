namespace InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
public class ModemDetails : EntityCharacteristicsDetailsBase
{
    public string DataRate { get; init; }
    public string ModemTechnology { get; init; }
    public string ConnectorType { get; init; }
}
