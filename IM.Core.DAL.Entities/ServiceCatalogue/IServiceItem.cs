using System;

namespace InfraManager.DAL.ServiceCatalogue
{
    public interface IServiceItem : ICatalog<Guid>
    {
        byte[] RowVersion { get; }
        string Parameter { get; }
        CatalogItemState? State { get; }
        string Note { get; }
        string ExternalID { get; }
        Guid? FormID { get; }
        Service Service { get; }
    }
}
