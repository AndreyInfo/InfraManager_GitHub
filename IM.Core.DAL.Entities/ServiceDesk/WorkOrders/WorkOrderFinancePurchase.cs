using System;
using InfraManager.DAL.Finance;

namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    public class WorkOrderFinancePurchase
    {
        public Guid WorkOrderID { get; }
        public string Concord { get; set;  }
        public string Bill { get; set; }
        public bool DetailBudget { get; set; }
        public DateTime? UtcDateDelivered { get; set; }
        public Guid? SupplierID { get; set; }
        public virtual Supplier Supplier { get; }
        public Guid? ResponsibleID { get; set; }
        public ObjectClass? ResponsibleClass { get; set; }

        protected WorkOrderFinancePurchase()
        {
        }

        public WorkOrderFinancePurchase(Guid workOrderId, string concord, string bill, bool detailBudget)
        {
            WorkOrderID = workOrderId;
            Concord = concord;
            Bill = bill;
            DetailBudget = detailBudget;
        }
    }
}