using System;

namespace InfraManager.BLL.Asset.PortAdapter;

public class PortAdapterData
{
    public Guid ObjectID { get; init; }
    public int PortNumber { get; init; }
    public int JackTypeID { get; init; }
    public int TechnologyID { get; init; }
    public string PortAddress { get; set; }
    public string Note { get; set; }
}