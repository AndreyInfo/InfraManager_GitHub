using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification
{
    /// <summary>
    /// Объект представляет узел дерева оповещений
    /// </summary>
    public class DefaultNotificationTreeItem
    {
        public int ID { get; init; }
        public Guid NotificationID { get; init; }
        public int ParentId { get; init; }
        public NotificationTreeLevel Level { get; init; }
        public bool Selected { get; set; }
        public string Name { get; init; }
        public bool HasChild { get; set; }
        public ObjectClass ClassId { get; set; }
        public bool PartSelected { get; set; }
    }
}
