using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Suppliers.SupplierContactPerson;

public class SupplierContactPersonFilter : BaseFilter
{
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Phone { get; init; }
    public string Email { get; init; }
}