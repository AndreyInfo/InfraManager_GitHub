if 'AdapterPort' not in (select [TABLE_NAME] from [INFORMATION_SCHEMA].[TABLES] where [TABLE_SCHEMA] = 'dbo')
CREATE TABLE [dbo].[AdapterPort] (
	[ID] uniqueidentifier default NEWID(),
	[ObjectID] uniqueidentifier not null,
	[JackTypeID] int NOT NULL,
	[TechnologyTypeID] int NOT NULL,
	[Number] int not null,
	[PortAddress] nvarchar(250) null,
	[Note] nvarchar(250) null,
	constraint PK_AdapterPort primary key clustered ([ID]),
	constraint FK_AdapterPort_JackType foreign key ([JackTypeID]) references [dbo].[Виды разъемов]([Идентификатор]),
	constraint FK_AdapterPort_TechnologyType foreign key ([TechnologyTypeID]) references [dbo].[Виды технологий]([Идентификатор])
)