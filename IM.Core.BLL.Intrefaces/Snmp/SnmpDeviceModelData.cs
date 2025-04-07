using System;

namespace InfraManager.BLL.Snmp
{
    public class SnmpDeviceModelData
    {
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
        public Guid ProfileID { get; init; }

        /// <summary>
        /// Наименование модель
        /// </summary>
        public string ModelName { get; init; }

        /// <summary>
        /// Модель – ссылка на модель в каталоге продуктов ('Типы активного оборудования'/'active_equipment_types')
        /// </summary>
        public int ModelID { get; init; }

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
