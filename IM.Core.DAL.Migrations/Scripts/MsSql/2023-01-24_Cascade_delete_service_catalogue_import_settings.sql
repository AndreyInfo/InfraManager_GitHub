if EXISTS (SELECT 1
					FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C
					WHERE C.TABLE_NAME = 'ServiceCatalogueImportSetting'
						AND C.CONSTRAINT_SCHEMA = 'dbo' 
						AND C.CONSTRAINT_NAME = 'FK_ServiceCatalogueImportSetting_ServiceCatalogueImportCSVConfiguration')
BEGIN 
ALTER TABLE [dbo].[ServiceCatalogueImportSetting]
drop CONSTRAINT FK_ServiceCatalogueImportSetting_ServiceCatalogueImportCSVConfiguration;
END

IF (OBJECT_ID(N'dbo.FK_ServiceCatalogueImportSetting_ServiceCatalogueImportCSVConfiguration', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[ServiceCatalogueImportSetting] WITH NOCHECK
        ADD CONSTRAINT [FK_ServiceCatalogueImportSetting_ServiceCatalogueImportCSVConfiguration]
        FOREIGN KEY ([ServiceCatalogueImportCSVConfigurationID])
        REFERENCES [dbo].[ServiceCatalogueImportCSVConfiguration] ([ID])
		ON DELETE SET NULL;
	
    ALTER TABLE [dbo].[ServiceCatalogueImportSetting] CHECK CONSTRAINT [FK_ServiceCatalogueImportSetting_ServiceCatalogueImportCSVConfiguration]
END