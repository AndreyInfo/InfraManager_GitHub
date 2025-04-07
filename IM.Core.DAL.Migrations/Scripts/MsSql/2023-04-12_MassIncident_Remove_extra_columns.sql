if 'MassIncidentSupervisor' in (select [TABLE_NAME] from INFORMATION_SCHEMA.TABLES where [TABLE_SCHEMA] = 'dbo')		
	drop table [dbo].[MassIncidentSupervisor]
go

if 'MassiveIncidentCallService' in (select [TABLE_NAME] from INFORMATION_SCHEMA.TABLES where [TABLE_SCHEMA] = 'dbo')		
	drop table [dbo].[MassiveIncidentCallService]
go

if 'ShortDescription' in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'MassIncident')
	alter table [dbo].[MassIncident] drop column [ShortDescription];
go
	
if 'ShortDescriptionPlain' in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'MassIncident')
	alter table [dbo].[MassIncident] drop column [ShortDescriptionPlain];
go
	
if 'UtcCompletedAt' in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'MassIncident')
	alter table [dbo].[MassIncident] drop column [UtcCompletedAt];
go
	
if 'ExecutorUserID' in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'MassIncident')
	exec sp_rename N'dbo.MassIncident.ExecutorUserID', N'ExecutedByUserID', N'COLUMN';
go

if 'GroupID' in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'MassIncident')
	exec sp_rename N'dbo.MassIncident.GroupID', N'ExecutedByGroupID', N'COLUMN';
go
	
if 'FullDescription' in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'MassIncident')
	exec sp_rename N'dbo.MassIncident.FullDescription', N'Description', N'COLUMN';
go
	
if 'FullDescriptionPlain' in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'MassIncident')
	exec sp_rename N'dbo.MassIncident.FullDescriptionPlain', N'DescriptionPlain', N'COLUMN';
go
	