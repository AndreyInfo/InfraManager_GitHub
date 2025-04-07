namespace InfraManager.UI.Web.PathBuilders
{
    [ObjectClassMapping(ObjectClass.ChangeRequest)]
    public class ChangeRequestsPathBuilder : DefaultResourcePathBuilder
    {
        protected override string Name => "changeRequests";
    }
}
