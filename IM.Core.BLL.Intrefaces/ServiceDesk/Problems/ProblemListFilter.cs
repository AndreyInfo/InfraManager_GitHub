using System;
using System.Collections.Generic;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    public class ProblemListFilter
    {
        public int? Number { get; init; }
        public IEnumerable<Guid> IDs { get; init; }
        public bool? ShouldSearchFinished { get; init; }
        public Guid? ExecutorID { get; init; }
        public bool? ShouldSearchAccomplished { get; init; }
    }
}