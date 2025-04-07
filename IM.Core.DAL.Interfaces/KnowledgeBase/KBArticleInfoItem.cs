using System;

namespace InfraManager.DAL.KnowledgeBase
{
    public class KBArticleInfoItem
    {
        public Guid ID { get; init; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string HTMLDescription { get; set; }

        public string HTMLSolution { get; set; }

        public DateTime UtcDateModified { get; set; }

        public string Tags { get; set; }
    }
}
