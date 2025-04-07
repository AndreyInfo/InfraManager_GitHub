using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.KnowledgeBase
{
    [ListViewItem(ListView.KBAAdmittedPersons)]
    public class KBArticleAccessListModel
    {
        public Guid KbArticleID { get; set; }

        public Guid ObjectID { get; init; }

        public ObjectClass ObjectClass { get; init; }

        [ColumnSettings(0, 250)]
        [Label(nameof(Resources.KBAHasAccess))]
        public string ObjectName { get; set; }

        [ColumnSettings(1)]
        [Label(nameof(Resources.KBANestedDivisions))]
        public bool WithSub { get; set; }
    }
}
