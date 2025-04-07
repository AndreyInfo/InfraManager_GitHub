if 'MassiveIncidentInformationChannel' not in (select[TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[MassiveIncidentInformationChannel] (
        [ID] smallint not null,
        [Name] nvarchar(50) not null,
		constraint PK_MassiveIncidentInformationChannel primary key clustered ([ID]))		
go

if 1 not in (select [ID] from [dbo].[MassiveIncidentInformationChannel])
	insert into [dbo].[MassiveIncidentInformationChannel] ([ID], [Name]) values (1, 'Monitoring')
go

if 2 not in (select [ID] from [dbo].[MassiveIncidentInformationChannel])
	insert into [dbo].[MassiveIncidentInformationChannel] ([ID], [Name]) values (2, 'User')
go

if 3 not in (select [ID] from [dbo].[MassiveIncidentInformationChannel])
	insert into [dbo].[MassiveIncidentInformationChannel] ([ID], [Name]) values (3, 'Administrator')
go

if 'MassiveIncidentCause' not in (select [TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[MassiveIncidentCause] (
        [ID] int not null identity(1,1),
        [Name] nvarchar(500) not null,
		[Removed] bit not null default(0),
		[RowVersion] timestamp not null,
        constraint PK_MassiveIncidentCause primary key clustered ([ID]))		
go

if 'MassiveIncidentType' not in (select [TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[MassiveIncidentType] (
        [ID] int not null identity(1,1),
        [Name] nvarchar(255) not null,
		[WorkflowSchemeIdentifier] nvarchar(255) null,
		[FormID] uniqueidentifier null,
		[Removed] bit not null default(0),
		[RowVersion] timestamp not null,
        constraint PK_MassiveIncidentType primary key clustered ([ID]))		
go

if 'MassiveIncident' not in (select [TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[MassiveIncident] (
        [ID] int not null identity(1,1),
        [IMObjID] uniqueidentifier not null default(newid()),
		[Name] nvarchar(255) not null,
		[ShortDescriptionPlain] nvarchar(1000),
		[FullDescriptionPlain] nvarchar(1000),
		[SolutionPlain] nvarchar(1000),
		[ShortDescription] nvarchar(4000),
		[FullDescription] nvarchar(4000),
		[Solution] nvarchar(4000),
		[InformationChannelID] smallint not null,
        [PriorityID] uniqueidentifier not null,
        [CriticalityID] uniqueidentifier null,
        [TypeID] int not null,
        [CauseID] int null,
        [UtcCreatedAt] datetime not null,
        [UtcLastModifiedAt] datetime not null,
        [UtcOpenedAt] datetime null,
        [UtcCompletedAt] datetime null,
        [UtcCloseUntil] datetime null,
		[UtcRegisteredAt] datetime null,
        [CreatedByUserID] int not null,
        [OwnedByUserID] int null,
        [GroupID] uniqueidentifier not null,
        [SlaID] uniqueidentifier null,
		[CallServiceID] uniqueidentifier not null,
		[EntityStateID] [nvarchar](50) NULL,
		[EntityStateName] [nvarchar](250) NULL,
		[WorkflowSchemeID] [uniqueidentifier] NULL,
		[WorkflowSchemeIdentifier] [nvarchar](50) NULL,
		[WorkflowSchemeVersion] [nvarchar](50) NULL,
        [RowVersion] timestamp not null,
        constraint PK_MassiveIncident primary key clustered ([ID]),
		constraint UK_MassiveIncident_IMObjID unique nonclustered ([IMObjID]),
		constraint FK_MassiveIncident_InformationChannel foreign key ([InformationChannelID]) references [dbo].[MassiveIncidentInformationChannel]([ID]),
		constraint FK_MassiveIncident_Priority foreign key ([PriorityID]) references [dbo].[Priority]([ID]),
		constraint FK_MassiveIncident_Criticality foreign key ([CriticalityID]) references [dbo].[Criticality]([ID]),
		constraint FK_MassiveIncident_Type foreign key ([TypeID]) references [dbo].[MassiveIncidentType]([ID]),
		constraint FK_MassiveIncident_Cause foreign key ([CauseID]) references [dbo].[MassiveIncidentCause]([ID]),
		constraint FK_MassiveIncident_CreatedBy foreign key ([CreatedByUserID]) references [dbo].[Пользователи]([Идентификатор]),
		constraint FK_MassiveIncident_OwnedBy foreign key ([OwnedByUserID]) references [dbo].[Пользователи]([Идентификатор]),
		constraint FK_MassiveIncident_Group foreign key ([GroupID]) references [dbo].[Queue]([ID]),
		constraint FK_MassiveIncident_SlaID foreign key ([SlaID]) references [dbo].[SLA]([ID]),
		constraint FK_MassiveIncident_CallServiceID foreign key ([CallServiceID]) references [dbo].[CallService]([ID]));
go

if not 980 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (980, 823, 'MassiveIncident.Properties', 'Открыть свойства', 'Операция позволяет просматривать свойства объекта Массовый инцидент через форму свойств, но не позволяет изменять.');
go

if not 981 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (981, 704, 'MassiveIncident.AllMassiveIncidentsList', 'Вкладка Массовые инциденты', 'Операция предоставляет доступ к списку "Все массовые инциденты"');
go

if not 982 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (982, 823, 'MassiveIncident.Add', 'Создать', 'Операция дает возможность создавать новый объект Массовый инцидент, но не дает возможности просмотра и изменения объекта Массовый инцидент.');
go

if not 983 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (983, 823, 'MassiveIncident.Update', 'Сохранить', 'Операция позволяет изменять поля объекта Массовый инцидент через форму свойств, а также его состояния.');
go

if not 984 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (984, 823, 'MassiveIncident.Delete', 'Удалить', 'Операция дает возможность удалять объект Массовый инцидент.');
go

if not 985 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (985, 824, 'MassiveIncidentType.Properties', 'Свойства', 'Операция дает возможность просмотра данных типов массовых инцидентов');
go

if not 986 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (986, 824, 'MassiveIncidentType.Add', 'Создать', 'Операция дает возможность создания типов массовых инцидентов');
go

if not 987 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (987, 824, 'MassiveIncidentType.Update', 'Редактировать', 'Операция дает возможность редактирования типов массовых инцидентов');
go

if not 988 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (988, 824, 'MassiveIncidentType.Delete', 'Удалить', 'Операция дает возможность удаления типов массовых инцидентов');
go

if not 989 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (989, 825, 'MassiveIncidentCause.Properties', 'Свойства', 'Операция дает возможность просмотра данных причин массовых инцидентов');
go

if not 990 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (990, 825, 'MassiveIncidentCause.Add', 'Создать', 'Операция дает возможность создания причин массовых инцидентов');
go

if not 991 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (991, 825, 'MassiveIncidentCause.Update', 'Редактировать', 'Операция дает возможность редактирования причин массовых инцидентов');
go

if not 992 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (992, 825, 'MassiveIncidentCause.Delete', 'Удалить', 'Операция дает возможность удаления причин массовых инцидентов');
go

insert into [dbo].[RoleOperation] ([OperationID], [RoleID])
select t.[ID], '00000000-0000-0000-0000-000000000001'
from [dbo].[Operation] t
left join [dbo].[RoleOperation] x on x.OperationID = t.[ID] and x.[RoleID] = '00000000-0000-0000-0000-000000000001'
where (t.[ID] between 980 and 992) and (x.OperationID is null)
go

if 'MassiveIncidentSupervisor' not in (select[TABLE_NAME] from [INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[MassiveIncidentSupervisor] (
        [ID] bigint not null identity(1,1),
        [MassiveIncidentID] int not null,
		[UserID] int not null,
		constraint PK_MassiveIncidentSupervisor primary key clustered ([ID]),
		constraint UK_MassiveIncidentSupervisor unique nonclustered ([MassiveIncidentID], [UserID]),
		constraint FK_MassiveIncidentSupervisor_MassiveIncident foreign key ([MassiveIncidentID]) references [dbo].[MassiveIncident]([ID]),
		constraint FK_MassiveIncidentSupervisor_User foreign key ([UserID]) references [dbo].[Пользователи]([Идентификатор]))
go


if 'MassiveIncidentCall' not in (select[TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[MassiveIncidentCall] (
        [ID] bigint not null identity(1,1),
        [MassiveIncidentID] int not null,
		[CallID] uniqueidentifier not null,
		constraint PK_MassiveIncidentCall primary key clustered ([ID]),
		constraint UK_MassiveIncidentCall unique nonclustered ([MassiveIncidentID], [CallID]),
		constraint FK_MassiveIncidentCall_MassiveIncident foreign key ([MassiveIncidentID]) references [dbo].[MassiveIncident]([ID]),
		constraint FK_MassiveIncidentCall_Call foreign key ([CallID]) references [dbo].[Call]([ID]))
go

if 'MassiveIncidentProblem' not in (select[TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[MassiveIncidentProblem] (
        [ID] bigint not null identity(1,1),
        [MassiveIncidentID] int not null,
		[ProblemID] uniqueidentifier not null,
		constraint PK_MassiveIncidentProblem primary key clustered ([ID]),
		constraint UK_MassiveIncidentProblem unique nonclustered ([MassiveIncidentID], [ProblemID]),
		constraint FK_MassiveIncidentProblem_MassiveIncident foreign key ([MassiveIncidentID]) references [dbo].[MassiveIncident]([ID]),
		constraint FK_MassiveIncidentProblem_Problem foreign key ([ProblemID]) references [dbo].[Problem]([ID]))
go

if 'MassiveIncidentChangeRequest' not in (select[TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[MassiveIncidentChangeRequest] (
        [ID] bigint not null identity(1,1),
        [MassiveIncidentID] int not null,
		[ChangeRequestID] uniqueidentifier not null,
		constraint PK_MassiveIncidentChangeRequest primary key clustered ([ID]),
		constraint UK_MassiveIncidentChangeRequest unique nonclustered ([MassiveIncidentID], [ChangeRequestID]),
		constraint FK_MassiveIncidentChangeRequest_MassiveIncident foreign key ([MassiveIncidentID]) references [dbo].[MassiveIncident]([ID]),
		constraint FK_MassiveIncidentChangeRequest_ChangeRequest foreign key ([ChangeRequestID]) references [dbo].[RFC]([ID]))
go

if 'MassiveIncidentCallService' not in (select[TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[MassiveIncidentCallService] (
        [ID] bigint not null identity(1,1),
        [MassiveIncidentID] int not null,
		[CallServiceID] uniqueidentifier not null,
		constraint PK_MassiveIncidentCallService primary key clustered ([ID]),
		constraint UK_MassiveIncidentCallService unique nonclustered ([MassiveIncidentID], [CallServiceID]),
		constraint FK_MassiveIncidentCallService_MassiveIncident foreign key ([MassiveIncidentID]) references [dbo].[MassiveIncident]([ID]),
		constraint FK_MassiveIncidentCallService_CallService foreign key ([CallServiceID]) references [dbo].[CallService]([ID]))
go

if 125 not in (select [ID] from [dbo].[Setting])
	insert into [dbo].[Setting] ([ID], [Value]) values (125, 0x)
go

if 'AllMassiveIncidentsList' not in (select [ViewName] from [dbo].[WebFilters] where [Name] = '_ALL_')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_ALL_', 1, 'AllMassiveIncidentsList', 1)
go