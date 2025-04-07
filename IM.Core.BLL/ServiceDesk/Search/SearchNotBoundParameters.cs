using System;
using System.Collections.Generic;

namespace InfraManager.BLL.ServiceDesk.Search
{
    public class SearchNotBoundParameters : ServiceDeskSearchParameters
    {
        public SearchNotBoundParameters(Guid parentID, IReadOnlyList<ObjectClass> classes) : base(classes)
        {
            ParentID = parentID;
        }

        public Guid ParentID { get; }
    }
}