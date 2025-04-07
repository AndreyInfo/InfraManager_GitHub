using System;
using Inframanager.BLL;

namespace InfraManager.BLL.ServiceDesk.WorkOrders
{
    public class WorkOrderFinancePurchaseData
    {
        public NullablePropertyWrapper<Guid> SupplierID { get; init; }
        public DateTime? UtcDateDelivered { get; init; }
        public bool DetailBudget { get; init; }
        public string Concord { get; init; }
        public string Bill { get; init; }
    }
}