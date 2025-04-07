using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification
{
    /// <summary>
    /// Объект для сохранения ролей из дерева оповещений
    /// </summary>
    public class NotificationSaveData
    {
        public Guid ID { get; init; }
        public int Role { get; init; }
        public bool Checked { get; init; }
    }

}
