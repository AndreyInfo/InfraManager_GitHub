using System;

namespace InfraManager.BLL.Suppliers.SupplierContactPerson;

public class SupplierContactPersonDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Patronymic { get; init; }
    public string Phone { get; init; }
    public string SecondPhone { get; init; }
    public string Email { get; init; }
    public Guid? PositionID { get; init; }
    public string PositionName { get; init; }
    public string Note { get; init; }
    public Guid SupplierID { get; init; }
    public string SupplierName { get; init; }
}
