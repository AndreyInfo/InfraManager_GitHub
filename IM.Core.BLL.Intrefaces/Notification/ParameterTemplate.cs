using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification
{
    /// <summary>
    /// Шаблон параметра
    /// </summary>
    public class ParameterTemplate
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; init; }
        /// <summary>
        /// Шаблон
        /// </summary>
        public string Template { get; init; }
    }
}
