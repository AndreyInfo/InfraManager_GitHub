using System;

namespace InfraManager.BLL.Snmp
{
    public sealed class SnmpDeviceModelDetails : SnmpDeviceModelData
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid ID { get; init; }
    }
}
