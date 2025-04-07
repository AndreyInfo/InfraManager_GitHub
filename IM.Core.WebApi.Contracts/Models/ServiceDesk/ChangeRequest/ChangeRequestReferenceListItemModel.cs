using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents
{
    public class ChangeRequestReferenceListItemModel
    {
        public Guid ID => IMObjID;
        public Guid IMObjID { get; init; }
        public ObjectClass ClassID => ObjectClass.ChangeRequest;
        public int Number { get; init; }
        public string TypeName { get; init; }
        public string ShortDescription { get; init; }
        public string EntityStateName { get; init; }
        public string Owner { get; init; }
        public string Priority { get; init; }
    }
}
