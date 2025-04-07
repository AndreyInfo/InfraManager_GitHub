namespace InfraManager.UI.Web.PathBuilders
{
    [ObjectClassMapping(ObjectClass.Problem)]
    public class ProblemPathBuilder : DefaultResourcePathBuilder
    {
        protected override string Name => "problems";
    }
}
