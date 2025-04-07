using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ELP
{
    [ListViewItem(ListView.ELPTaskVendorList)]
    public sealed class ELPVendorListItem
    {
        /// <summary>
        /// Идентификатор связи
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [ColumnSettings(1)]
        [Label(nameof(Resources.Name))]
        public string Name { get; set; }
    }
}