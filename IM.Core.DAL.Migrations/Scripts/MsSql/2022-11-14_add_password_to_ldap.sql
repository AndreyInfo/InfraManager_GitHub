IF NOT EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'password' AND Object_ID = Object_ID(N'dbo.UIADPath'))
BEGIN
    alter table UIADPath add [password] nvarchar(4000) null;
END
