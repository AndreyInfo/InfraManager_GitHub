if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_ServiceCatalogueImportSetting_Name' and object_id = OBJECT_ID('ServiceCatalogueImportSetting')))
        BEGIN
            CREATE UNIQUE INDEX UI_ServiceCatalogueImportSetting_Name on ServiceCatalogueImportSetting (Name);
        end;