using System;
using System.Collections.Generic;

namespace InfraManager.Web.Controllers.Models.LicenceSchemes
{
    /// <summary>
    /// Запрос на удаление схем лицензирования
    /// </summary>
    public sealed class LicenceSchemeMarkAsDeletedRequest
    {
        /// <summary>
        /// Идентификаторы схем лицензирования
        /// </summary>
        public List<Guid> ObjectIdList { get; set; }
    }
}