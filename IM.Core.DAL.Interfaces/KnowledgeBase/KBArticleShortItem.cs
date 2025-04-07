using System;

namespace InfraManager.DAL.KnowledgeBase
{
    public class KBArticleShortItem
    {
        public Guid ID { get; init; }

        public string Name { get; set; }

        public int? Number { get; set; }

        public DateTime UtcDateCreated { get; set; }

        public DateTime UtcDateModified { get; set; }

        public Guid AuthorId { get; set; }

        public string Description { get; set; }

        public string Solution { get; set; }

        public bool Visible { get; set; }

        public int DocumentsCount { get; set; }

        public string AuthorFullName { get; set; }

        public string Section { get; set; }
    }
}
