-- Change Primary Key in TechnicalFailuresCategory from integer to uniqueidentifier

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'TechnicalFailuresCategory' AND [COLUMN_NAME] = 'IMObjID'))

	BEGIN

		ALTER TABLE [dbo].[TechnicalFailuresCategory] ADD [IMObjID] uniqueidentifier NOT NULL default(newid());
		ALTER TABLE [dbo].[TechnicalFailuresCategory] ADD CONSTRAINT UK_TechnicalFailuresCategory_IMObjID UNIQUE ([IMObjID]);

	END


-- Change Primary Key in MassiveIncidentCause from integer to uniqueidentifier
IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
   WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'MassiveIncidentCause' AND [COLUMN_NAME] = 'IMObjID'))
   
   BEGIN

	   ALTER TABLE [dbo].[MassiveIncidentCause] ADD [IMObjID] uniqueidentifier NOT NULL default(newid());
	   ALTER TABLE [dbo].[MassiveIncidentCause] ADD CONSTRAINT UK_MassiveIncidentCause_IMObjID UNIQUE ([IMObjID]);

   END