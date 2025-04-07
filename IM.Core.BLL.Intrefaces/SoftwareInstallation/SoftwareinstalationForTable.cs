using System;
using Inframanager.BLL;
using InfraManager.ResourcesArea;
using Inframanager.BLL.ListView;

namespace InfraManager.BLL.Software
{
    [ListViewItem(ListView.SoftwareInstallationForTable)]
    public class SoftwareinstalationForTable
    {
        [ColumnSettings(1, 200)]
        [Label(nameof(Resources.Identifier))]
        public Guid ID { get; set; }

        [ColumnSettings(2, 351)]
        [Label(nameof(Resources.software_installation_column_name))]
        public string SoftwareModelName { get; set; }

        [ColumnSettings(3, 200)]
        [Label(nameof(Resources.software_installation_column_product))]
        public string CommercialModelName { get; set; }

        [ColumnSettings(4, 200)]
        [Label(nameof(Resources.software_installation_column_installation_location))]
        public string DeviceName { get; set; }

        [ColumnSettings(5, 200)]
        [Label(nameof(Resources.software_installation_column_installation_path))]
        public string InstallPath { get; set; }

        [ColumnSettings(6, 200)]
        [Label(nameof(Resources.software_installation_column_date_creation))]
        public string CreateDate { get; set; }

        [ColumnSettings(7, 200)]
        [Label(nameof(Resources.software_installation_column_date_last_rights_check))]
        public string DateLastRightsCheck { get; set; }

        [ColumnSettings(8, 200)]
        [Label(nameof(Resources.software_installation_column_date_last_survey))]
        public string DateLastSurvey { get; set; }

        [ColumnSettings(9, 200)]
        [Label(nameof(Resources.software_installation_column_installation_date))]
        public string InstallDate { get; set; }

        [ColumnSettings(10, 100)]
        [Label(nameof(Resources.software_installation_column_status_name))]
        public string StatusName { get; set; }
    }
}
