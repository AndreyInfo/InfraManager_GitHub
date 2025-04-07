using System;
using System.Collections.Generic;
using System.Text;

namespace IM.Core.WebApi.Contracts.Common.Models
{
    /// <summary>
    /// Список идентифткиторов
    /// </summary>
    public class GuidList
    {
        /// <summary>
        /// Конретный список идентификаторов
        /// </summary>
        public Guid[] IDList { get; set; }

    }
}
