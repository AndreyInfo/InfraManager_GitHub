using System;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Components;

public class SoftwareModelComponentListItemDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Version { get; init; }
    public string ExternalID { get; init; }
}
