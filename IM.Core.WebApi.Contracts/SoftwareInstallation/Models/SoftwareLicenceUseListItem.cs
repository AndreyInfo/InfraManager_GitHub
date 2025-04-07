using System;

namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareInstallation.Models
{
    /// <summary>
    /// Модель элемента списка инсталляций
    /// </summary>
    public sealed class SoftwareLicenceUseListItem
    {
        /// <summary>
        /// Идентификатор лицензии
        /// </summary>
        public  Guid ID { get; set; }

        /// <summary>
        /// Названеие лицензии
        /// </summary>       
        public string SoftwareLicenceName { get; set; }

        /// <summary>
        /// Названеие объекта потребления
        /// </summary>       
        public string ObjectName { get; set; }

        /// <summary>
        /// Кол-во прав
        /// </summary>       
        public int? RightCount { get; set; }
    }
}
