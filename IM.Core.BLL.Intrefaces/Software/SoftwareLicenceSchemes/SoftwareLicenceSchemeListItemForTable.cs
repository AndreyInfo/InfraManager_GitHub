using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.Software.SoftwareLicenceSchemes
{
    [ListViewItem(ListView.SoftwareLicenseSchemeForTable)]
    public class SoftwareLicenceSchemeListItemForTable
    {

        [ColumnSettings(1, 200)]
        [Label(nameof(Resources.LicenceSchemesColumnName))]
        public string Name { get; set; }

        [ColumnSettings(2, 200)]
        [Label(nameof(Resources.LicenceSchemesColumnSoftwareLicenceObject))]
        public string LicensingObjectTypeLabel { get; set; }

        /// <summary>
        /// Вид схемы. Представление
        /// </summary>
        [ColumnSettings(3, 200)]
        [Label(nameof(Resources.LicenceSchemesColumnSoftwareLicenceSchemeType))]
        public string SchemeTypeLabel { get; set; }

        /// <summary>
        /// Дата создания схемы
        /// </summary>
        [ColumnSettings(4, 200)]
        [Label(nameof(Resources.LicenceSchemesColumnCreatedDate))]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Дата последней модификации схемы
        /// </summary>
        [ColumnSettings(5, 200)]
        [Label(nameof(Resources.LicenceSchemesColumnUpdatedDate))]
        public DateTime UpdatedDate { get; set; }
    }
}