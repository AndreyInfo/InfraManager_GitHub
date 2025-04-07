using System;

namespace InfraManager.DAL;

public interface IImportPostLinkParameters
{
    
    Guid ProductCatalogTypeID { set; }

    int ManufacturerID { set; }
}