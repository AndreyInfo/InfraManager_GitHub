if 'MassiveIncident' in (select [TABLE_NAME] from INFORMATION_SCHEMA.TABLES where [TABLE_SCHEMA] = 'dbo')
	EXEC sp_rename 'dbo.MassiveIncident', 'MassIncident';
go

if 'MassIncidentNote' not in (select [TABLE_NAME] from INFORMATION_SCHEMA.TABLES where [TABLE_SCHEMA] = 'dbo')
	create table [dbo].[MassIncidentNote] (
		[ID] uniqueidentifier not null, -- Это неправильный тип ПК, особенно для данной таблицы, но времени переделывать правильно нет => за скорость разработки платим быстродействием
		[MassIncidentID] uniqueidentifier not null, -- Ссылка на UK в таблице массовых инцидентов, а не на ПК, опять же из-за спешки
		[CreatedByUserID] uniqueidentifier not null, -- Ссылка на UK в таблице пользователей, а не на ПК, опять же из-за спешки
		[CreatedAt] datetime not null,
		[TextPlain] nvarchar(4000) not null,
		[TextFormatted] nvarchar(4000) not null,
		[Type] tinyint not null,
		constraint PK_MassIncidentNote primary key nonclustered ([ID]),
		constraint FK_MassIncidentNote_MassIncident foreign key ([MassIncidentID]) references [dbo].[MassIncident] ([IMObjID]),
		constraint FK_MassIncidentNote_Author foreign key ([CreatedByUserID]) references [dbo].[Пользователи] ([IMObjID])
	);
go

if 'TechnicalFailureCategoryID' not in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_SCHEMA] = 'dbo' and [TABLE_NAME] = 'MassIncident')
begin
	alter table [dbo].[MassIncident] add [TechnicalFailureCategoryID] int null;
	alter table [dbo].[MassIncident] 
		add constraint [FK_MassIncident_TechnicalFailureCategory] 
		foreign key ([TechnicalFailureCategoryID]) 
		references [TechnicalFailuresCategory] ([ID]);
end
go