using InfraManager.DAL.Software;
using System;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Installations;

public class SoftwareModelInstallationListItemDetails
{
    public Guid ID { get; init; }
    public string SoftwareModelName { get; init; }
    public Guid DeviceID { get; init; }
    public string InstallPath { get; init; }
    public DateTime? InstallDate { get; init; }
    public DateTime UtcDateCreated { get; init; }
    public DateTime? UtcDateLastDetected { get; init; }
    public string StateName { get; init; }
}
