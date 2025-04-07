using System;

namespace InfraManager.UI.Web.Models.ServiceCatalogue
{
    public class ServiceCategoryViewModel
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Note { get; init; }
        public string ImageSource { get; init; }
        public ServiceViewModel[] ServiceList { get; init; }
    }
}
