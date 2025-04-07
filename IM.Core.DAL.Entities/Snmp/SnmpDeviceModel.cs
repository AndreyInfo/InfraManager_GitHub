using Inframanager;
using System;

namespace InfraManager.DAL.Snmp
{
    [ObjectClassMapping(ObjectClass.SnmpDeviceModel)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.SnmpDeviceModel_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.SnmpDeviceModel_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.SnmpDeviceModel_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.SnmpDeviceModel_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.SnmpDeviceModel_Properties)]
    /// <summary>
    /// Объект «Правило распознавания»
    /// </summary>
    public class SnmpDeviceModel
    {

        protected SnmpDeviceModel()
        {
        }

        public SnmpDeviceModel(string modelName)
        {
            ModelName = modelName;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid ID { get; init; }

        /// <summary>
        /// Дополнительный OID - строка
        /// </summary>
        public string OID { get; init; }

        /// <summary>
        /// Ответ по OID - строка
        /// </summary>
        public string OIDValue { get; init; }

        /// <summary>
        /// Ответ по sysObjectID - строка
        /// </summary>
        public string SysObjectIDValue { get; init; }

        /// <summary>
        /// Тэг из sysDescr - строка
        /// </summary>
        public string DescriptionTag { get; init; }

        /// <summary>
        /// Профиль – ссылка на справочник профилей
        /// </summary>
        public Guid? ProfileID { get; init; }

        /// <summary>
        /// Профиль
        /// </summary>
        public virtual SnmpDeviceProfile Profile { get; init; }

        /// <summary>
        /// Наименование модель
        /// </summary>
        public string ModelName { get; init; }

        /// <summary>
        /// Модель – ссылка на модель в каталоге продуктов ('Типы активного оборудования'/'active_equipment_types')
        /// </summary>
        public int ModelID { get; init; }

        //public virtual object Model { get; set; } // TODO: обновить тип ('Типы активного оборудования'/'active_equipment_types')

        /// <summary>
        /// Производитель
        /// </summary>
        public string ManufacturerName { get; init; }

        /// <summary>
        /// Шаблон
        /// </summary>
        public int Template { get; init; }

        /// <summary>
        /// Примечание – текст
        /// </summary>
        public string Note { get; init; }
        
        /// <summary>
        /// Игнорировать?
        /// </summary>
        public bool IsIgnore { get; init; }
        
        /// <summary>
        /// Синхронизирован?
        /// </summary>
        public bool IsSynchronized { get; init; }
        
        /// <summary>
        /// Версия записи
        /// </summary>
        public byte[] RowVersion { get; init; }
    }
}
