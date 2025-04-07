using System;

namespace InfraManager.BLL.ServiceDesk.Manhours
{
    public class ManhoursListFilter
    {
        public InframanagerObject? Parent { get; init; }
        public Guid? ExecutorID { get; init; }
    }
}