using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.Snmp
{
    [ListViewItem(ListView.SnmpDeviceModel)]
    public class SnmpDeviceModelListItem
    {
        public ObjectClass ClassID => ObjectClass.SnmpDeviceModel;

        public Guid ID { get; init; }

        /// <summary>
        /// Наименование модель
        /// </summary>
        [ColumnSettings(1)]
        [Label(nameof(Resources.Name))]
        public string ModelName { get; init; }

        /// <summary>
        /// Дополнительный OID - строка
        /// </summary>
        [ColumnSettings(2)]
        public string OID { get; set; }

        /// <summary>
        /// Ответ по OID - строка
        /// </summary>
        [ColumnSettings(3)]
        public string OIDValue { get; set; }

        /// <summary>
        /// Ответ по sysObjectID - строка
        /// </summary>
        [ColumnSettings(4)]
        public string SysObjectIDValue { get; set; }

        /// <summary>
        /// Тэг из sysDescr - строка
        /// </summary>
        [ColumnSettings(5)]
        public string DescriptionTag { get; set; }
    }
}
