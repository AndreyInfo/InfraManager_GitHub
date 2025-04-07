IF EXISTS (SELECT * FROM information_schema.constraint_column_usage WHERE [TABLE_NAME] = 'ITAssetImportSetting' AND [CONSTRAINT_NAME] = 'FK_ITAssetImportSetting_Workplace')

	ALTER TABLE [dbo].[ITAssetImportSetting] DROP CONSTRAINT FK_ITAssetImportSetting_Workplace;

GO
	
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = 'ITAssetImportSetting' AND column_name = 'DefaultWorkplaceID' AND data_type = 'uniqueidentifier')
	
	ALTER TABLE [dbo].[ITAssetImportSetting] ALTER COLUMN [DefaultWorkplaceID] VARCHAR (50);
	ALTER TABLE [dbo].[ITAssetImportSetting] ALTER COLUMN [DefaultWorkplaceID] uniqueidentifier;
	
GO

IF EXISTS (SELECT * FROM information_schema.constraint_column_usage WHERE [TABLE_NAME] = 'ITAssetImportSetting' AND [CONSTRAINT_NAME] = 'FK_ITAssetImportSetting_Workflow')

	ALTER TABLE [dbo].[ITAssetImportSetting] DROP CONSTRAINT FK_ITAssetImportSetting_Workflow;

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = 'ITAssetImportSetting' AND column_name = 'WorkflowID' AND data_type = 'nvarchar')

	ALTER TABLE [dbo].[ITAssetImportSetting] ALTER COLUMN [WorkflowID] VARCHAR (50);

GO