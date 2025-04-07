using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification
{
    /// <summary>
    /// Действие правила
    /// </summary>
    public enum RecipientScope : byte
    {
        /// <summary>
        /// Все
        /// </summary>
        All = 0,
        /// <summary>
        /// Объект
        /// </summary>
        Object = 1,
    }
}
