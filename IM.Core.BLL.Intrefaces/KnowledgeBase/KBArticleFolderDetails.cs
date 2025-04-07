using System;

namespace InfraManager.BLL.KnowledgeBase
{
    public class KBArticleFolderDetails
    {
        public Guid ID { get; init; }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public bool Visible { get; set; }

        public bool HasChilds { get; set; }

        public Guid? ParentID { get; set; }
    }
}
