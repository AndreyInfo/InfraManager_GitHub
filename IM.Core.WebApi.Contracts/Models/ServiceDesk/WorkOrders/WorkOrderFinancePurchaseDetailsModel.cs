using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.WorkOrders;

public class WorkOrderFinancePurchaseDetailsModel
{
    public Guid? SupplierID { get; init; }
    public string SupplierName { get; init; }
    public string UtcDateDelivered { get; init; }
    public bool DetailBudget { get; init; }
    public string Concord { get; init; }
    public string Bill { get; init; }

    //TODO: посчитать, когда будет реализован функционал закупок
    public string PurchasedAndPlacedCostWithNDS => "0%";
    public string WaitPurchaseCostWithNDS => "100%";
}