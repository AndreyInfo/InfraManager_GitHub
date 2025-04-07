using InfraManager.BLL.ProductCatalogue.Models.AdditionalFields;
using InfraManager.DAL.ProductCatalogue;
using System;

namespace InfraManager.BLL.ProductCatalogue.Models;

public class ProductModelOutputDetails
{
    public Guid ID { get; init; }

    public Guid ProductCatalogTypeID { get; set; }
    
    public ProductTemplate TemplateID { get; set; }
    
    public ObjectClass TemplateClassID { get; set; }
    
    public ObjectClass? ModelClassID { get; set; }

    public string TemplateClassName { get; set; }
    
    public string CategoryName { get; set; }
    
    public string ProductCatalogTypeName { get; set; }
    
    public string Name { get; init; }
    
    public string ProductNumber { get; set; }
    
    public string Code { get; set; }
    
    public string Note { get; init; }
    
    public string LifeCycleName { get; init; }
    
    public string VendorName { get; init; }
        
    public string ExternalID { get; init; }
    
    public bool IsLogical { get; init; }
    
    public int? VendorID { get; set; }

    public Guid? ManufactureID { get; set; }
    
    public string RowVersion { get; init; }

    public IProductCatalogModelProperties Properties { get; init; }


    //TODO оставить для дополнения полей моделей
    #region ProductClassTabs
    //public int? Ports { get; set; }
    //public int? Expence { get; set; }
    //public int? Adapters { get; set; }
    //public int? Slots { get; set; }
    //public ProductClassTabs Tabs { get; set; }
    #endregion

    #region ProductCatalogModelCharacteristics
    //public bool Power { get; set; }
    //public bool ReadSpeed { get; set; }
    //public bool WriteSpeed { get; set; }
    //public bool CommonMemoryLength { get; set; }
    //public bool Modes { get; set; }
    //public bool ConverterType { get; set; }
    //public bool ChipType { get; set; }
    //public bool Mirror { get; set; }
    //public bool Interface { get; set; }
    //public bool PartitionsCount { get; set; }
    //public bool Size { get; set; }
    //public bool Storage { get; set; }
    //public bool DMA { get; set; }
    //public bool Interrution { get; set; }
    //public bool MacWwn { get; set; }
    //public bool MaximumFrequency { get; set; }
    //public bool MemorySlots { get; set; }
    //public bool SecondaryBusType { get; set; }
    //public bool PrimaryBusType { get; set; }
    //public bool Chipset { get; set; }
    //public bool ModemTechnology { get; set; }
    //public bool TransferSpeed { get; set; }
    //public bool ConnectionPortType { get; set; }
    //public bool MemorySize { get; set; }
    //public bool Desination { get; set; }
    //public bool HeadsCount { get; set; }
    //public bool SectorsCount { get; set; }
    //public bool CylindersCount { get; set; }
    //public bool CoreCount { get; set; }
    //public bool Platform { get; set; }
    //public bool SecondLevelCacheSize { get; set; }
    //public bool StartFrequency { get; set; }
    //public bool CurrentFrequency { get; set; }
    //public bool SocketType { get; set; }
    //public bool Speed { get; set; }
    //public ProductCatalogModelCharacteristics Characteristics { get; set; }
    #endregion
}
