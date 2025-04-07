using System;

namespace InfraManager.DAL.KnowledgeBase
{
    public class KBArticleFolderItem
    {
        public Guid ID { get; init; }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public bool Visible { get; set; }

        public bool HasChilds { get; set; }

        public Guid? ParentId { get; set; }
    }
}
