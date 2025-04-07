using System;

namespace InfraManager.BLL.KnowledgeBase
{
    public class KBArticleShortDetails
    {
        public Guid ID { get; init; }

        public string Name { get; set; }

        public int? Number { get; set; }

        public string UtcDateCreated { get; set; }

        public string UtcDateModified { get; set; }

        public Guid AuthorID { get; set; }

        public string Description { get; set; }

        public string Solution { get; set; }

        public bool Visible { get; set; }

        public int DocumentsCount { get; set; }

        public string AuthorFullName { get; set; }

        public string Section { get; set; }
    }
}
