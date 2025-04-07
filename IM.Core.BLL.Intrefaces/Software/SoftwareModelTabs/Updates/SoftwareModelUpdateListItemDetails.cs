using System;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Updates;

public class SoftwareModelUpdateListItemDetails
{
    public Guid ID { get; init; }
    public string Version { get; init; }
    public string ExternalID { get; init; }
    public string ParentModelName { get; init; }
    public string ParentModelVersion { get; init; }
}
