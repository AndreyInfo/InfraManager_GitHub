using Inframanager;
using System;

namespace InfraManager.DAL.Asset;

[OperationIdMapping(ObjectAction.Insert, OperationID.TelephoneType_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.TelephoneType_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.TelephoneType_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.TelephoneType_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.TelephoneType_Properties)]
public class PhoneType : Catalog<Guid>
{
    public Guid? ComplementaryID { get; set; }
}
