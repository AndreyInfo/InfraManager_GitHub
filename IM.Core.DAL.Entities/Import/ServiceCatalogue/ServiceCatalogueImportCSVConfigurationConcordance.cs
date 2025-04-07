using System;

namespace InfraManager.DAL.Import.ServiceCatalogue
{
    public class ServiceCatalogueImportCSVConfigurationConcordance
    {
        public ServiceCatalogueImportCSVConfigurationConcordance(Guid serviceCatalogueImportCSVConfigurationID, string field, string expression)
        {
            ServiceCatalogueImportCSVConfigurationID = serviceCatalogueImportCSVConfigurationID;
            Field = field;
            Expression = expression;
        }

        public Guid ServiceCatalogueImportCSVConfigurationID { get; set; }
        public string Field { get; init; }
        public string Expression { get; set; }
        public virtual ServiceCatalogueImportCSVConfiguration Configuration { get; init; }
    }
}
