using System;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    public class ChangeRequestListFilter
    {
        public int? Number { get; init; }
        public bool? ShouldSearchFinished { get; init; }
        public Guid[] IDs { get; init; }
        public Guid? ProblemID { get; init; }
    }
}
