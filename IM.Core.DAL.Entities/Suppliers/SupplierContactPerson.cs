using Inframanager;
using InfraManager.DAL.OrganizationStructure;
using System;
using SupplierEntity = InfraManager.DAL.Finance.Supplier;

namespace InfraManager.DAL.Suppliers;

[ObjectClassMapping(ObjectClass.SupplierContactPerson)]
[OperationIdMapping(ObjectAction.Insert, OperationID.SupplierContactPerson_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.SupplierContactPerson_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.SupplierContactPerson_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.SupplierContactPerson_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.SupplierContactPerson_Properties)]
public class SupplierContactPerson
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Patronymic { get; init; }
    public string Phone { get; init; }
    public string SecondPhone { get; init; }
    public string Email { get; init; }
    public Guid? PositionID { get; init; }
    public string Note { get; init; }
    public Guid SupplierID { get; init; }
    public byte[] RowVersion { get; init; }

    public virtual SupplierEntity Supplier { get; init; }
    public virtual JobTitle Position { get; init; }
}
