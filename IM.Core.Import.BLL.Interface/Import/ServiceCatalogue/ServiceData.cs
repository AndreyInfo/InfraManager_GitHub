using InfraManager.DAL.ServiceCatalogue;
namespace IM.Core.Import.BLL.Interface.Import.ServiceCatalogue
{
    public class ServiceData
    {
        public ServiceData(string name, Guid? categoryID, CatalogItemState state, string externalID, string categoryName)
        {
            Name = name;
            CategoryID = categoryID;
            State = state;
            ExternalID = externalID;
            CategoryName = categoryName;
        }

        public string Name { get; init; }
        public Guid? CategoryID { get; set; }
        public CatalogItemState State { get; init; }
        public string ExternalID { get; init; }
        public string CategoryName { get; init; }

    }
}
