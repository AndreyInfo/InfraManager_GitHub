namespace InfraManager.UI.Web.Models.Filters
{
    public class FilterModel
    {
        public string Name { get; set; }
        public string View { get; set; }
        public FilterElementModel[] Elements { get; set; }
    }
}
