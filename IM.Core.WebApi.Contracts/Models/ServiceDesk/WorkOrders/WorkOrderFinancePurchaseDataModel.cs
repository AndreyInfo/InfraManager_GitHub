using System;
using Inframanager.BLL;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.WorkOrders
{
    public class WorkOrderFinancePurchaseDataModel
    {
        public NullablePropertyWrapper<Guid> SupplierID { get; init; }
        public string UtcDateDelivered { get; init; }
        public bool DetailBudget { get; init; }
        public string Concord { get; init; }
        public string Bill { get; init; }
    }
}