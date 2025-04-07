using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Installations;

[ListViewItem(ListView.SoftwareModelInstallationForTable)]
public class SoftwareModelInstallationForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.SoftwareInstallationGUID))]
    public string ID { get; set; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.software_installation_column_name))]
    public string SoftwareModelName { get; set; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.software_installation_column_installation_location))]
    public string DeviceID { get; set; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.software_installation_column_installation_path))]
    public string InstallPath { get; set; }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.software_installation_column_installation_date))]
    public string InstallDate { get; set; }

    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.software_installation_column_date_creation))]
    public string UtcDateCreated { get; set; }

    [ColumnSettings(7, 100)]
    [Label(nameof(Resources.software_installation_column_date_last_survey))]
    public string UtcDateLastDetected { get; set; }

    [ColumnSettings(8, 100)]
    [Label(nameof(Resources.software_installation_column_status_name))]
    public string StateName { get; set; }
}
