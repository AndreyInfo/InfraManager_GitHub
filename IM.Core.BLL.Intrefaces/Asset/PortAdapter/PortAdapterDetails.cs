using System;

namespace InfraManager.BLL.Asset.PortAdapter;

public class PortAdapterDetails
{
    public Guid ID { get; init; }
    public Guid ObjectID { get; init; }
    public int PortNumber { get; init; }
    public int JackTypeID { get; init; }
    public string JackTypeName { get; init; }
    public int TechnologyID { get; init; }
    public string TechnologyTypeName { get; init; }
    public string PortAddress { get; set; }
    public string Note { get; set; }
}
