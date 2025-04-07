if 'ITAssetImportCSVConfiguration' not in (select [TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[ITAssetImportCSVConfiguration] (
		[ID] uniqueidentifier not null default(newid()),
		[Name] nvarchar(250) not null,
		[Note] nvarchar(500) null,
		[Delimiter] nvarchar(1) not null,
		constraint PK_ITAssetImportCSVConfiguration primary key clustered ([ID]),
		constraint UK_ITAssetImportCSVConfiguration_Name unique nonclustered ([Name]))	
go

if 'ITAssetImportCSVConfigurationClassConcordance' not in (select [TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[ITAssetImportCSVConfigurationClassConcordance] (
		[ITAssetImportCSVConfigurationID] uniqueidentifier not null,
		[Field] nvarchar(50) not null,
		[Expression] nvarchar(2048) not null,
		constraint PK_ITAssetImportCSVConfigurationClassConcordance primary key clustered ([ITAssetImportCSVConfigurationID], [Field]),
		constraint FK_ITAssetImportCSVConfigurationClassConcordance_ImportCSV foreign key ([ITAssetImportCSVConfigurationID]) references [dbo].[ITAssetImportCSVConfiguration]([ID]) ON DELETE CASCADE)	
go

if 'ITAssetImportCSVConfigurationFieldConcordance' not in (select [TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[ITAssetImportCSVConfigurationFieldConcordance] (
		[ITAssetImportCSVConfigurationID] uniqueidentifier not null,
		[Field] nvarchar(50) not null,
		[Expression] nvarchar(2048) not null,
		constraint PK_ITAssetImportCSVConfigurationFieldConcordance primary key clustered ([ITAssetImportCSVConfigurationID], [Field]),
		constraint FK_ITAssetImportCSVConfigurationFieldConcordance_ImportCSV foreign key ([ITAssetImportCSVConfigurationID]) references [dbo].[ITAssetImportCSVConfiguration]([ID]) ON DELETE CASCADE)	
go

if 'ITAssetImportSetting' not in (select [TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[ITAssetImportSetting] (
		[ID] uniqueidentifier not null default(newid()),
		[Name] nvarchar(250) not null,
		[Note] nvarchar(500) null,
		[ITAssetImportCSVConfigurationID] uniqueidentifier not null,
		[Path] nvarchar(500) not null,
		[CreateModelAutomatically] bit not null default(0),
		[DefaultModelID] uniqueidentifier null,
		[MissingModelInSource] bit not null default(0),
		[MissingTypeInSource] bit not null default(0),
		[UnsuccessfulAttemptToCreateAutomatically] bit not null default(0),
		[AsdToWorkplaceOfUser] bit not null default(0),
		[DefaultWorkplaceID] int null,
		[CreateDeviation] bit not null default(0),
		[CreateMessages] bit not null default(0),
		[CreateSummaryMessages] bit not null default(0),
		[WorkflowID] uniqueidentifier null,
		constraint PK_ITAssetImportSetting primary key clustered ([ID]),
		constraint UK_ITAssetImportSetting_Name unique nonclustered ([Name]),
		constraint FK_ITAssetImportSetting_ImportCSV foreign key ([ITAssetImportCSVConfigurationID]) references [dbo].[ITAssetImportCSVConfiguration]([ID]) ON DELETE CASCADE,
		constraint FK_ITAssetImportSetting_Workplace foreign key ([DefaultWorkplaceID]) references [dbo].[Рабочее место]([Идентификатор]) ON DELETE SET NULL,
		constraint FK_ITAssetImportSetting_Workflow foreign key ([WorkflowID]) references [dbo].[Workflow]([ID]) ON DELETE SET NULL)	
go