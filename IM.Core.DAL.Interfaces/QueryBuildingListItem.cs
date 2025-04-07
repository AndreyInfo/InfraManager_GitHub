using System;

namespace InfraManager.DAL
{
    /// <summary>
    /// Результат отбора при поиске зданий
    /// </summary>
    public class QueryBuildingListItem
    {
        /// <summary>
        /// Guid активного устройства
        /// </summary>
        public Guid? ActiveDeviceImobjId { get; set; }

        /// <summary>
        /// Guid оконечного оборудования при условии равенства ИдКомнаты идентификатору комнаты
        /// </summary>
        public Guid? TerminalEquipment1ImobjId { get; set; }

        /// <summary>
        /// Guid оконечного оборудования при условии равенства ИдРабочегоМеста идентификатору комнаты
        /// </summary>
        public Guid? TerminalEquipment2ImobjId { get; set; }        
    }
}
