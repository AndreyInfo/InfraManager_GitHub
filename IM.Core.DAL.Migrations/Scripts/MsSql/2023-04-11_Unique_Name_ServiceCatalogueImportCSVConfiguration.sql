if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_ServiceCatalogueImportCSVConfiguration_Name' and object_id = OBJECT_ID('ServiceCatalogueImportCSVConfiguration')))
        BEGIN
            CREATE UNIQUE INDEX UI_ServiceCatalogueImportCSVConfiguration_Name on ServiceCatalogueImportCSVConfiguration (Name);
        end;