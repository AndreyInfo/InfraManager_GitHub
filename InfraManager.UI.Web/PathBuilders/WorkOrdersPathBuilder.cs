namespace InfraManager.UI.Web.PathBuilders
{
    [ObjectClassMapping(ObjectClass.WorkOrder)]
    public class WorkOrdersPathBuilder : DefaultResourcePathBuilder
    {
        protected override string Name => "workOrders";
    }
}
