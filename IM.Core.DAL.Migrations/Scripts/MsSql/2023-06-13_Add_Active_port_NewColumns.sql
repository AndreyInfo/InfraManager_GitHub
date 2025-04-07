if 'Name' not in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'Порт активный')
begin
	alter table [dbo].[Порт активный]
	add [Name] nvarchar(250) null;
end
go
if 'PortModule' not in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'Порт активный')
begin
	alter table [dbo].[Порт активный]
	add [PortModule] UNIQUEIDENTIFIER null;
end
go
if 'JackTypeID' not in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'Порт активный')
begin
	alter table [dbo].[Порт активный]
	add [JackTypeID] INT null;
end