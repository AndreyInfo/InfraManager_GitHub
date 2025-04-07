using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using System;

namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models
{
    /// <summary>
    /// Модель данных схемы лицензирования
    /// </summary>
    [EntityCompare(750, (int)ObjectClass.LicenceScheme, "Схема Лицензирования", AddOperationID = 750002, EditOperationID = 750003, DeleteOperationID = 750004)]
    public sealed class SoftwareLicenceScheme
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [FieldCompare("Наименование", 1)]
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [FieldCompare("Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Вид схемы
        /// </summary>
        [FieldCompare("Вид схемы")]
        public SoftwareLicenceSchemeType SchemeType { get; set; }

        /// <summary>
        /// Объект лицензирования 
        /// </summary>
        [FieldCompare("Объект лицензирования", SetFieldProperty = "id")]
        public SoftwareLicenceSchemeObjectTypes LicensingObjectType { get; set; }

        /// <summary>
        /// Требуемое количество прав на объект. Формула расчета 
        /// </summary>
        [FieldCompare("Требуемое количество прав на объект")]
        public string LicenseCountPerObject { get; set; }

        /// <summary>
        /// Привязывать права к объектам 
        /// </summary>
        [FieldCompare("Привязывать права к объектам", SetFieldProperty = "val")]
        public bool IsLinkLicenseToObject { get; set; }

        /// <summary>
        /// Лицензировать все хосты кластеров (для кластеров) 
        /// </summary>
        [FieldCompare("Лицензировать все хосты кластеров (для кластеров)", SetFieldProperty = "val")]
        public bool IsLicenseAllHosts { get; set; }

        /// <summary>
        /// Лицензировать доступ пользователей 
        /// </summary>
        [FieldCompare("Лицензировать доступ пользователей", SetFieldProperty = "val")]
        public bool IsLinkLicenseToUser { get; set; }

        /// <summary>
        /// Допустимое число инсталляций на устройстве 
        /// </summary>
        [FieldCompare("Допустимое число инсталляций на устройстве", SetFieldProperty ="val")]
        public int? InstallationLimits { get; set; }

        /// <summary>
        /// Число инсталляций на виртуальных машинах 
        /// </summary>
        [FieldCompare("Число инсталляций на виртуальных машинах", SetFieldProperty = "val")]
        public int InstallationLimitPerVM { get; set; }

        /// <summary>
        /// Число инсталляций на виртуальных машинах : запрещено, разрешено без или с ограничением
        /// </summary>
        [FieldCompare("Число инсталляций на виртуальных машинах : запрещено, разрешено без или с ограничением")]
        public SoftwareLicenceSchemeForVM AllowInstallOnVM { get; set; }

        /// <summary>
        /// Лицензия на доступ 
        /// </summary>
        [FieldCompare("Лицензия на доступ")]
        public bool IsLicenceByAccess { get; set; }

        /// <summary>
        /// Количество дополнительных прав. Формула расчета 
        /// </summary>
        [FieldCompare("Количество дополнительных прав", NullValueLabel ="без ограничений")]
        public string AdditionalRights { get; set; }

        /// <summary>
        /// Размер увеличения инсталляций на виртуальных машинах 
        /// </summary>
        [FieldCompare("Размер увеличения инсталляций на виртуальных машинах", SetFieldProperty = "val")]
        public int IncreaseForVM { get; set; }

        /// <summary>
        /// Дата-время создания
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Дата-время последнего изменения
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Призанк удаленной схемы
        /// </summary>
        [FieldCompare("Призанк удаленной схемы")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Вид схемы. Представление
        /// </summary>
        public string SchemeTypeLabel { get; set; }

        /// <summary>
        /// Объект лицензирования. Представление 
        /// </summary>
        public string LicensingObjectTypeLabel { get; set; }

        /// <summary>
        /// Список коэффициентов схемы.
        /// </summary>
        [FieldCompare("Коэффициент", SetFieldProperty ="", ListIDField = nameof(SoftwareLicenceSchemeCoefficient.ProcessorTypeID))]
        public SoftwareLicenceSchemeCoefficient[] SoftwareLicenceSchemeCoefficients { get; set; }
    }
}
