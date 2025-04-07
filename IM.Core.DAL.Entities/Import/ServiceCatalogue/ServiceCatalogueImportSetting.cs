using System;


namespace InfraManager.DAL.Import.ServiceCatalogue
{
    public class ServiceCatalogueImportSetting
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public string Note { get; init; } 
        public Guid? ServiceCatalogueImportCSVConfigurationID { get; set; }
        public string Path { get; init; }
        public byte[] RowVersion { get; init; }  
        public virtual ServiceCatalogueImportCSVConfiguration ServiceCatalogueImportCSVConfiguration { get; init; }
    }
}
