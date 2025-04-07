using Inframanager;
using System;

//TODO проверить правильность выставление операций
namespace InfraManager.DAL.OrganizationStructure
{
    [ObjectClassMapping(ObjectClass.Owner)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.Owner_Update)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Owner_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Owner_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class Owner : IGloballyIdentifiedEntity
    {
        public static readonly Guid DefaultOwnerID = Guid.Empty;

        public Guid IMObjID { get; }
        public string Name { get; set; }
        public int? VisioID { get; set; }        
    }
}
