using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Settings
{
    /// <summary>
    /// Фильтр для краткого описания заявок
    /// </summary>
    public class HtmlTagWorkerFilter : BaseFilter
    {
        /// <summary>
        /// Идентификатор правила цитирования
        /// </summary>
        public Guid EmailQuoteTrimmerId { get; set; }
    }
}
