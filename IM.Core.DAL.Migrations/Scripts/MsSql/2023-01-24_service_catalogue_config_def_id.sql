ALTER TABLE ServiceCatalogueImportCSVConfiguration 
ADD  CONSTRAINT [DF_ServiceCatalogueImportCSVConfiguration]  
DEFAULT NEWID() FOR ID;