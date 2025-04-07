IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'RowVersion'
          AND Object_ID = Object_ID(N'dbo.[FormValues]'))
BEGIN
    ALTER TABLE dbo.[FormValues]
	ADD RowVersion timestamp NOT NULL
END

IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'RowVersion'
          AND Object_ID = Object_ID(N'dbo.[Values]'))
BEGIN
    ALTER TABLE dbo.[Values]
	ADD RowVersion timestamp NOT NULL
END