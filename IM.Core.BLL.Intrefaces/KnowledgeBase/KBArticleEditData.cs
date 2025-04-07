using System;

namespace InfraManager.BLL.KnowledgeBase
{
    public class KBArticleEditData
    {
        //TODO dima: move to Inframanager.Core and use at InfraManager.CL.Win
        public const string DefaultDocument = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"><HTML><HEAD><META content=\"text/html; charset=utf-8\" http-equiv=Content-Type></HEAD><BODY>{0}</BODY></HTML>";

        public string Name { get; set; }

        public string HTMLDescription { get; set; }

        public string HTMLSolution { get; set; }

        public string HTMLAlternativeSolution { get; set; }

        public string AlternativeSolution { get; set; }

        public string Description { get; set; }

        public string Solution { get; set; }

        public bool Visible { get; set; }

        public Guid StatusID { get; set; }

        public Guid TypeID { get; set; }

        public Guid AccessID { get; set; }

        public Guid? ExpertID { get; set; }

        public DateTime? UtcDateValidUntil { get; init; }

        public string[] Tags { get; set; }

        public Guid[] KBArticleDependencyList { get; set; }
    }
}
