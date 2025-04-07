using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;


namespace InfraManager.BLL.Notification
{
    [ListViewItem(ListView.NotificationsList)]
    public class NotificationsListItem
    {
        public Guid ID { get { return default; } }

        [ColumnSettings(0, 100)]
        [Label(nameof(Resources.Name))]
        public string Name { get { return default; } }

        [ColumnSettings(1, 100)]
        [Label(nameof(Resources.Type))]
        public string ObjectType { get { return default; } }

        [ColumnSettings(2, 100)]
        [Label(nameof(Resources.UserNote))]
        public string Note { get { return default; } }
    }
}
