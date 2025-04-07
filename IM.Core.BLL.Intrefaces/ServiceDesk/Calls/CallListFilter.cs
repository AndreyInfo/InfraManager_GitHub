using System;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public class CallListFilter
    {
        public int? Number { get; init; }
        public bool? ShouldSearchFinished { get; init; }
        public Guid[] IDs { get; init; }
        public bool? ShouldSearchAccomplished { get; init; }
        public Guid? OwnerID { get; init; }
        public Guid? ExecutorID { get; init; }
    }
}