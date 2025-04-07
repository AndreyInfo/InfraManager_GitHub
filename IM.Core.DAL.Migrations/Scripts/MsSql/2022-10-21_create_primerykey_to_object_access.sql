
IF NOT EXISTS( SELECT 1 FROM sys.columns
				WHERE Name = N'ID'
				AND Object_ID = Object_ID(N'dbo.ObjectAccess'))
BEGIN

	ALTER TABLE [ObjectAccess]
		ADD ID uniqueidentifier not null default(NEWID());

	ALTER TABLE [dbo].[ObjectAccess]
		ADD CONSTRAINT PK_ObjectAccess PRIMARY KEY(ID);
    
END
GO