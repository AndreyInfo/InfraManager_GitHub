namespace InfraManager.UI.Web
{
    public abstract class DefaultResourcePathBuilder : IBuildResourcePath
    {
        protected abstract string Name { get; }

        public string GetPathToList()
        {
            return $"api/{Name}/";
        }

        public string GetPathToSingle(string id)
        {
            return $"api/{Name}/{id}";
        }
    }
}
