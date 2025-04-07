using System;

namespace InfraManager.DAL.ServiceDesk
{
    internal class ExecutableInfo
    {
        public Guid IMObjID { get; init; }
        public Guid GroupID { get; init; }
        public Guid ExecutorID { get; init; }
    }
}
