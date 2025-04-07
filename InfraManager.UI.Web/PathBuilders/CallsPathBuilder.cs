namespace InfraManager.UI.Web.PathBuilders
{
    [ObjectClassMapping(ObjectClass.Call)]
    public class CallsPathBuilder : DefaultResourcePathBuilder
    {
        protected override string Name => "calls";
    }
}
