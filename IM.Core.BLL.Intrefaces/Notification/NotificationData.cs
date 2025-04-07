using System;

namespace InfraManager.BLL.Notification
{
    /// <summary>
    /// Оповещение
    /// </summary>
    public class NotificationData
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Note { get; init; }
        public string ObjectType { get; init; }
    }
}
