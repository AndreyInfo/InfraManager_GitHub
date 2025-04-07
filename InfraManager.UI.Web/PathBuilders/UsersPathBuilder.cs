namespace InfraManager.UI.Web.PathBuilders
{
    [ObjectClassMapping(ObjectClass.User)]
    public class UsersPathBuilder : DefaultResourcePathBuilder
    {
        protected override string Name => "users";
    }
}
