using System;

namespace InfraManager.BLL.KnowledgeBase
{
    public class KBArticleInfoDetails
    {
        public Guid ID { get; init; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string HTMLDescription { get; set; }

        public string HTMLSolution { get; set; }

        public string TagString { get; set; }

        // TODO: 
        // public List<KBFile> KBFiles { get; set; }
    }
}
