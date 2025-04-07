if EXISTS (SELECT 1
					FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C
					WHERE C.TABLE_NAME = 'ServiceCatalogueImportCSVConfigurationConcordance'
						AND C.CONSTRAINT_SCHEMA = 'dbo' 
						AND C.CONSTRAINT_NAME = 'FK_ServiceCatalogueImportCSVConfigurationConcordance_ServiceCatalogueImportCSVConfiguration')
BEGIN 
ALTER TABLE [dbo].[ServiceCatalogueImportCSVConfigurationConcordance]
drop CONSTRAINT FK_ServiceCatalogueImportCSVConfigurationConcordance_ServiceCatalogueImportCSVConfiguration;
END

IF (OBJECT_ID(N'dbo.FK_ServiceCatalogueImportCSVConfigurationConcordance_ServiceCatalogueImportCSVConfiguration', 'F') IS NULL)
BEGIN
    ALTER TABLE [dbo].[ServiceCatalogueImportCSVConfigurationConcordance] WITH NOCHECK
        ADD CONSTRAINT [FK_ServiceCatalogueImportCSVConfigurationConcordance_ServiceCatalogueImportCSVConfiguration]
        FOREIGN KEY ([ServiceCatalogueImportCSVConfigurationID])
        REFERENCES [dbo].[ServiceCatalogueImportCSVConfiguration] ([ID])
		ON DELETE CASCADE ON UPDATE NO ACTION;
	
    ALTER TABLE [dbo].[ServiceCatalogueImportCSVConfigurationConcordance] CHECK CONSTRAINT [FK_ServiceCatalogueImportCSVConfigurationConcordance_ServiceCatalogueImportCSVConfiguration]
END