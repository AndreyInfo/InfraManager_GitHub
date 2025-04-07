using System;

namespace InfraManager.BLL.ELP
{
    public sealed class ELPSettingDetails
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid ID { get; init; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Описание
        /// </summary>
        public Guid? VendorID { get; init; }

        /// <summary>
        /// Производитель. Представление
        /// </summary>
        public string VendorName { get; init; }
    }
}
