using System;

namespace InfraManager.DAL.ServiceDesk.ChangeRequests
{
    public class ChangeRequestDependency : Dependency
    {
        protected ChangeRequestDependency() { }
        public ChangeRequestDependency(Guid changeRequestID, InframanagerObject inframanagerObject) : base(changeRequestID, inframanagerObject)
        {
        }
    }
}