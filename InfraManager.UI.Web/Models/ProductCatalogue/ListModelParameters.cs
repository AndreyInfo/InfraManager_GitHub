using System;

namespace InfraManager.UI.Web.Models.ProductCatalogue
{
    public class ListModelParameters
    {
        public Guid ParentID { get; init; }

        public int? StartIndex { get; init; }

        public int? Count { get; init; }
    }
}
