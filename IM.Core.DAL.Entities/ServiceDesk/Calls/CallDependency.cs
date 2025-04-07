using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class CallDependency : Dependency
    {
        protected CallDependency() { }
        public CallDependency(Guid callID, InframanagerObject inframanagerObject) : base(callID, inframanagerObject)
        {
        }
    }
}
