using Inframanager;
using System;

namespace InfraManager.DAL.Snmp
{
    [ObjectClassMapping(ObjectClass.SnmpDeviceProfile)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.SnmpDeviceProfile_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.SnmpDeviceProfile_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.SnmpDeviceProfile_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.SnmpDeviceProfile_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.SnmpDeviceProfile_Properties)]
    /// <summary>
    /// Профиль объекта «Правило распознавания»
    /// </summary>
    public class SnmpDeviceProfile
    {
        protected SnmpDeviceProfile()
        {
        }

        public SnmpDeviceProfile(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid ID { get; init; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Синхронизирован?
        /// </summary>
        public bool IsSynchronized { get; set; }

        /// <summary>
        /// Версия записи
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
