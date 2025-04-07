using System;


namespace InfraManager.DAL.Import.ServiceCatalogue
{
    public class ServiceCatalogueImportCSVConfiguration
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Note { get; init; }
        public string Delimeter { get; init; }
        public byte[] RowVersion { get; init; }
    }
}
