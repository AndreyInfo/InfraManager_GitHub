using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using System;

namespace InfraManager.CrossPlatform.WebApi.Contracts.ELP
{
    /// <summary>
    /// Модель данных схемы лицензирования
    /// </summary>
    public sealed class ELPVendorItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

    }
}
