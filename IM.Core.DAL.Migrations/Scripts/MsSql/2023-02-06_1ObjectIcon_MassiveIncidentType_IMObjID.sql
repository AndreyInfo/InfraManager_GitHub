if 'ObjectIcon' not in (select [TABLE_NAME] from INFORMATION_SCHEMA.TABLES where [TABLE_SCHEMA] = 'dbo')
begin
	create table [dbo].[ObjectIcon](		
		[ID] int NOT NULL identity(1,1),
		[ObjectID] uniqueidentifier NOT NULL,
		[ObjectClassID] int NOT NULL,
		[Name] nvarchar(100) NULL,
		[Content] image NULL,
		constraint PK_ObjectIcon primary key clustered ([ID]),
		constraint UK_ObjectIcon_Object unique([ObjectID], [ObjectClassID]));
end

if 'IMObjID' not in (select [COLUMN_NAME] from INFORMATION_SCHEMA.COLUMNS where [TABLE_NAME] = 'MassiveIncidentType' and [TABLE_SCHEMA] = 'dbo')
begin

	alter table [dbo].[MassiveIncidentType] 
		add [IMObjID] uniqueidentifier NOT NULL default(NEWID());
	alter table [dbo].[MassiveIncidentType] 
		add constraint UK_MassiveIncidentType_IMObjID
		unique([IMObjID])
end