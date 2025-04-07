using System;
using System.Collections.Generic;

namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models
{
    /// <summary>
    /// Запрос на пометку схем лицензирования удаленными
    /// </summary>
    public class SoftwareLicenceSchemeDeleteRequest
    {
        /// <summary>
        /// Список идентификаторов схем лицензирования
        /// </summary>
        public List<Guid> Guids { get; set; }
    }
}
