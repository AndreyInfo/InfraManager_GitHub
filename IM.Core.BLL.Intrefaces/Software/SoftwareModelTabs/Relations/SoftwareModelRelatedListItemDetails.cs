
using System;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Relations;

public class SoftwareModelRelatedListItemDetails
{
    public Guid ID { get; init; }
    public string Version { get; init; }
    public string ExternalID { get; init; }
    public string ParentName { get; init; }
    public string ParentVersion { get; init; }
}
