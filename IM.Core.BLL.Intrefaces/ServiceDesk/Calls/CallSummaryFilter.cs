using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    /// <summary>
    /// Фильтр для краткого описания заявок
    /// </summary>
    public class CallSummaryFilter : BaseFilter
    {
        /// <summary>
        /// Идентификатор типа объекта к которому относится ServiceId
        /// </summary>
        public ObjectClass ClassId { get; set; }

        /// <summary>
        /// Идентификатор сервиса/элемента/услуги
        /// </summary>
        public Guid? ServiceId { get; set; }

    }
}
