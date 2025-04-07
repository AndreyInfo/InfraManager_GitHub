using System;

namespace InfraManager.CrossPlatform.WebApi.Contracts.ELP
{
    /// <summary>
    /// Модель данных схемы лицензирования
    /// </summary>
    public sealed class ELPItem
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
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public Guid? VendorID { get; set; }

        /// <summary>
        /// Производитель. Представление
        /// </summary>
        public string VendorLabel { get; set; }

    }
}
