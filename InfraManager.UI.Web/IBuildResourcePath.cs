namespace InfraManager.UI.Web
{
    public interface IBuildResourcePath
    {
        string GetPathToList();
        string GetPathToSingle(string id);
    }
}
