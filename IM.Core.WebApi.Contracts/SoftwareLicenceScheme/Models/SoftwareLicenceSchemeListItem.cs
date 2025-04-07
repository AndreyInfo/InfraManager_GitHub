using System;

namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models
{
    /// <summary>
    /// Модель элемента схемы лицензирвоания для отображения в списке
    /// </summary>
    public sealed class SoftwareLicenceSchemeListItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Объект лицензирования 
        /// </summary>
        public SoftwareLicenceSchemeObjectTypes LicensingObjectType { get; set; }

        /// <summary>
        /// Объект лицензирования. Представление 
        /// </summary>
        public string LicensingObjectTypeLabel { get; set; }

        /// <summary>
        /// Вид схемы
        /// </summary>
        public SoftwareLicenceSchemeType SchemeType { get; set; }

        /// <summary>
        /// Вид схемы. Представление
        /// </summary>
        public string SchemeTypeLabel { get; set; }

        /// <summary>
        /// Дата создания схемы
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Дата последней модификации схемы
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Флаг удаленной схемы
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Флаг применимости ограничения расположения для данной схемы
        /// </summary>
        public bool IsLocationRestrictionApplicable { get; set; }
    }
}
