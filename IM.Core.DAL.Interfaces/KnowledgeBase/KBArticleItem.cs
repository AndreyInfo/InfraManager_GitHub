using System;

namespace InfraManager.DAL.KnowledgeBase
{
    public class KBArticleItem
    {
        public Guid ID { get; init; }

        public string Name { get; set; }

        public int? Number { get; set; }

        public string HTMLDescription { get; set; }

        public string HTMLSolution { get; set; }

        public string HTMLAlternativeSolution { get; set; }

        public string AlternativeSolution { get; set; }

        public int ViewsCount { get; set; }

        public int ApplicationCount { get; set; }

        public DateTime UtcDateCreated { get; set; }

        public DateTime UtcDateModified { get; set; }

        public Guid AuthorID { get; set; }

        public string Description { get; set; }

        public string Solution { get; set; }

        public bool Visible { get; set; }

        public int DocumentsCount { get; set; }

        public string AuthorFullName { get; set; }

        public int Readed { get; set; }

        public int Used { get; set; }

        public double Rated { get; set; }

        public Guid StatusID { get; set; }

        public string StatusName { get; set; }

        public Guid TypeID { get; set; }

        public string TypeName { get; set; }

        public bool VisibleForClient { get; set; }

        public Guid AccessID { get; set; }

        public string AccessName { get; set; }

        public Guid? ExpertID { get; set; }

        public string ExpertFullName { get; set; }

        public Guid? ModifierID { get; set; }

        public string ModifierFullName { get; set; }

        public DateTime? UtcDateValidUntil { get; init; }

        public Guid? LifeCycleStateID { get; set; }

        public string LifeCycleStateName { get; set; }

        public string[] KBADependencyList { get; set; }

        public string Section { get; set; }

        public string[] Tags { get; set; }
    }
}
