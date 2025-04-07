using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class WorkorderDependency : Dependency
    {
        protected WorkorderDependency() { }
        public WorkorderDependency(Guid workorderID, InframanagerObject inframanagerObject) : base(workorderID, inframanagerObject)
        {
        }

    }
}
