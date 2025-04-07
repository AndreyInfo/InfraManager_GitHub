namespace InfraManager.BLL.Asset.ConnectorTypes;

public class ConnectorTypeDetails
{
    public int ID { get; init; }
    public string Name { get; init; }
    public int MediumID { get; init; }
    public string MediumName { get; set; }
    public int PairCount { get; init; }
    public int? ComplementaryID { get; init; }
}