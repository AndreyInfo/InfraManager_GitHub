using System;

namespace InfraManager.BLL.ServiceDesk.WorkOrders;

public class WorkOrderFinancePurchaseDetails
{
    public Guid? SupplierID { get; init; }
    public string SupplierName { get; init; }
    public DateTime? UtcDateDelivered { get; init; }
    public bool DetailBudget { get; init; }
    public string Concord { get; init; }
    public string Bill { get; init; }
}