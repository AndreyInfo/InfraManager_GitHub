namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    public interface ICreateWorkOrderReference : IHaveWorkOrderReferences
    {
        WorkOrderReference CreateWorkOrderReference();
    }
}
