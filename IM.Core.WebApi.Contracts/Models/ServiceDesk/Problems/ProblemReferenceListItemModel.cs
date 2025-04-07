using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents
{
    public class ProblemReferenceListItemModel
    {
        public Guid ID => IMObjID;
        public Guid IMObjID { get; init; }
        public ObjectClass ClassID => ObjectClass.Problem;
        public int Number { get; init; }
        public string ShortDescription { get; init; }
        public string UtcDatePromised { get; init; }
        public string EntityStateName { get; init; }
        public string UtcDateModified { get; init; }
        public string Client { get; init; }
        public string TypeName { get; init; }
    }
}
