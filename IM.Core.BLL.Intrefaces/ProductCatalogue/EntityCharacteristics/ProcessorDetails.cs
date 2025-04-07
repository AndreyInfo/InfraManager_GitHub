namespace InfraManager.BLL.ProductCatalogue.ModelCharacteristics;
public class ProcessorDetails : EntityCharacteristicsDetailsBase
{
    public string MaxClockSpeed { get; init; }
    public string CurrentClockSpeed { get; init; }
    public string L2cacheSize { get; init; }
    public string SocketDesignation { get; init; }
    public string Platform { get; init; }
    public string NumberOfCores { get; init; }
}
