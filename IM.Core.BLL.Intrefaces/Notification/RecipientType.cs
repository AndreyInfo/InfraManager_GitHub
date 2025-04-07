using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification
{
    /// <summary>
    /// Тип получателя
    /// </summary>
    public enum RecipientType : byte
    {
        /// <summary>
        /// Почта
        /// </summary>
        Email = 0,
        /// <summary>
        /// Роль
        /// </summary>
        BusinessRole = 1,
    }
}
