if 'Description' in (select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'Call' and DATA_TYPE = 'ntext')
	alter table [dbo].[Call] alter column [Description] nvarchar(max) NULL
go

if 'Solution' in (select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'Call' and DATA_TYPE = 'ntext')
	alter table [dbo].[Call] alter column [Solution] nvarchar(max) NULL
go

if 'Description' in (select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'Problem' and DATA_TYPE = 'text')
	alter table [dbo].[Problem] alter column [Description] nvarchar(max) NULL -- странный выбор старого типа данных, почему везде может быть юникод, а у проблемы нет (заменяю на Nvarchar(max))
go

if 'Solution' in (select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'Problem' and DATA_TYPE = 'text')
	alter table [dbo].[Problem] alter column [Solution] nvarchar(max) NULL
go

if 'Cause' in (select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'Problem' and DATA_TYPE = 'text')
	alter table [dbo].[Problem] alter column [Cause] nvarchar(max) NULL
go

if 'Fix' in (select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'Problem' and DATA_TYPE = 'text')
	alter table [dbo].[Problem] alter column [Fix] nvarchar(max) NULL
go

if 'Description' in (select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'RFC' and DATA_TYPE = 'ntext')
	alter table [dbo].[RFC] alter column [Description] nvarchar(max) NULL
go