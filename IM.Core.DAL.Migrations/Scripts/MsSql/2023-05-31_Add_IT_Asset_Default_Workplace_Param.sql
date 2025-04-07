
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = 'ITAssetImportSetting' AND column_name = 'DefaultLocationNotSpecifiedID')

	ALTER TABLE [dbo].[ITAssetImportSetting] ADD [DefaultLocationNotSpecifiedID] BIT DEFAULT 'FALSE';

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = 'ITAssetImportSetting' AND column_name = 'DefaultLocationNotFoundID')

	ALTER TABLE [dbo].[ITAssetImportSetting] ADD [DefaultLocationNotFoundID] BIT DEFAULT 'FALSE';

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = 'ITAssetImportSetting' AND column_name = 'DefaultWorkplaceID')

	EXEC sp_rename '[dbo].[ITAssetImportSetting].[DefaultWorkplaceID]', 'DefaultLocationID', 'COLUMN';

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = 'ITAssetImportSetting' AND column_name = 'DefaultLocationID' AND data_type = 'uniqueidentifier')

	ALTER TABLE [dbo].[ITAssetImportSetting] ALTER COLUMN [DefaultLocationID] nvarchar(50);

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = 'ITAssetImportSetting' AND column_name = 'DefaultModelID' AND data_type = 'uniqueidentifier')

	ALTER TABLE [dbo].[ITAssetImportSetting] ALTER COLUMN [DefaultModelID] nvarchar(50);

GO

