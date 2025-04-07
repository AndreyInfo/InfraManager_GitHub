using System;
using Inframanager;

namespace InfraManager.DAL.ServiceDesk
{
    [ObjectClassMapping(ObjectClass.Problem)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Problem_Properties)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.Problem_Update)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Problem_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Problem_Update)]
    public class ProblemDependency : Dependency
    {
        public int ID { get; init; }

        protected ProblemDependency() { }
        public ProblemDependency(Guid problemID, InframanagerObject inframanagerObject) : base(problemID, inframanagerObject)
        {
        } 
    }
}
