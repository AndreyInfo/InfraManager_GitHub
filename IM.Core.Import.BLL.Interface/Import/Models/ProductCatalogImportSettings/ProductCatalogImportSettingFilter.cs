namespace IM.Core.Import.BLL.Interface.Import.Models.ProductCatalogImportSettings
{
    public class ProductCatalogImportSettingFilter
    {
        public string Name { get; init; }
        
        public string Note { get; init; }
        
        public bool RestoreRemovedModels { get; init; }
        
        public int? TechnologyTypeID { get; set; }
        
        public int? JackTypeID { get; set; }
        
        public Guid? ProductCatalogImportCSVConfigurationID { get; set; }
        
        public string Path { get; init; }
        
    }
}