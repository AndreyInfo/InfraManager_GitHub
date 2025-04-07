using System;
using System.Collections.Generic;
using Inframanager;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.DAL.ServiceDesk;

[ObjectClassMapping(ObjectClass.ServiceUnit)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ServiceUnit_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.ServiceUnit_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ServiceUnit_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ServiceUnit_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ServiceUnit_Properties)]
public class ServiceUnit : Catalog<Guid>
{
    public ServiceUnit()
    { }

    public ServiceUnit(string name, Guid responsibleID)
    {
        ID = Guid.NewGuid();
        Name = name;
        ResponsibleID = responsibleID;
    }

    public Guid ResponsibleID { get; set; }

    public byte[] RowVersion {get; set;}

    public virtual User ResponsibleUser { get; set; }

    public virtual ICollection<OrganizationItemGroup> OrganizationItemGroups { get; }
}
