using System;
using System.Collections.Generic;

namespace InfraManager.Web.Controllers.Models.LicenceSchemes
{
    /// <summary>
    /// Запрос на снятие пометки удаленное со схемы лицензирования
    /// </summary>
    public sealed class LicenceSchemeUnmarkAsDeletedRequest
    {
        /// <summary>
        /// Идентификаторы схем лицензирования
        /// </summary>
        public List<Guid> ObjectIdList { get; set; }
    }
}