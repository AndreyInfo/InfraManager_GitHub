IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'ITAssetImportSetting') AND 
	'NetworkAndTerminalIdenParam' NOT IN (SELECT [COLUMN_NAME] FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'ITAssetImportSetting') AND 
	'AdapterAndPeripheralIdenParam' NOT IN (SELECT [COLUMN_NAME] FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'ITAssetImportSetting') AND 
	'CUIdenParam' NOT IN (SELECT [COLUMN_NAME] FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'ITAssetImportSetting'))
   
	BEGIN
		
		ALTER TABLE [dbo].[ITAssetImportSetting] ADD [NetworkAndTerminalIdenParam] tinyint;
		ALTER TABLE [dbo].[ITAssetImportSetting] ADD [AdapterAndPeripheralIdenParam] tinyint;
		ALTER TABLE [dbo].[ITAssetImportSetting] ADD [CUIdenParam] tinyint;
		
	END