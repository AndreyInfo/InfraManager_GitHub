using System;
using System.Collections.Generic;
using Inframanager;

namespace InfraManager.DAL.ServiceDesk
{
    [ObjectClassMapping(ObjectClass.KBArticle)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.KBArticle_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.KBArticle_Properties)]
    public class KBArticle
    {
        public KBArticle()
        {
            ID = Guid.NewGuid();
        }

        public Guid ID { get; init; }

        public string Name { get; set; }

        public int? Number { get; set; }

        public DateTime UtcDateCreated { get; set; }

        public DateTime UtcDateModified { get; set; }

        public Guid AuthorID { get; set; }

        public string Description { get; set; }

        public string Solution { get; set; }

        public bool Visible { get; set; }

        public Guid ArticleAccessID { get; set; }

        public Guid ArticleStatusID { get; set; }

        public string HTMLDescription { get; set; }

        public string HTMLSolution { get; set; }

        public string HTMLAlternativeSolution { get; set; }

        public string AlternativeSolution { get; set; }

        public Guid ArticleTypeID { get; set; }

        public Guid? ExpertID { get; set; }

        public DateTime? UtcDateValidUntil { get; set; }

        public Guid? LifeCycleStateID { get; set; }

        public Guid ModifierID { get; set; }

        public int ViewsCount { get; set; }

        public virtual ICollection<KBArticleAccessList> AccessList { get; set; }
    }
}
