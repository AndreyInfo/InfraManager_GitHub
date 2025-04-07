if 'OperationLevelAgreement' not in (select [TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].OperationLevelAgreement (
        [ID] int not null identity(1,1),
	    [IMObjID] uniqueidentifier not null default(newid()),
        [Name] nvarchar(255) not null,
		[Number] nvarchar(255) not null,
		[UtcStartDate] datetime null,
		[UtcFinishDate] datetime null,
		[TimeZoneID] nvarchar(250) null,
		[CalendarWorkScheduleID] uniqueidentifier null,
		[Note] nvarchar(4000) null,
		[FormID] uniqueidentifier null,
		[RowVersion] timestamp null,

		constraint UI_OperationLevelAgreement_Name Unique(Name),
		constraint FK_OperationLevelAgreement_TimeZone foreign key([TimeZoneID]) references dbo.TimeZone(ID),
		constraint FK_OperationLevelAgreement_CalendarWorkSchedule foreign key(CalendarWorkScheduleID) references dbo.CalendarWorkSchedule(ID),
		constraint FK_OperationLevelAgreement_form foreign key (FormID) references dbo.WorkflowActivityForm(ID),
        constraint PK_OperationLevelAgreement primary key clustered ([ID]))		
go


if 'OperationLevelAgreementService' not in (select [TABLE_NAME] from[INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].OperationLevelAgreementService (
        [ID] bigint not null identity(1,1),
        [OperationLevelAgreementID] int not null,
		[ServiceID] uniqueidentifier not null,

		constraint PK_OperationLevelAgreementService primary key clustered ([ID]),
		constraint UK_OperationLevelAgreementService unique nonclustered ([OperationLevelAgreementID], [ServiceID]),
		constraint FK_OperationLevelAgreementService_OperationLevelAgreement foreign key ([OperationLevelAgreementID]) references [dbo].[OperationLevelAgreement]([ID]) on delete cascade,
		constraint FK_OperationLevelAgreementService_Service foreign key ([ServiceID]) references [dbo].[Service]([ID]) on delete cascade
		)
go


IF NOT EXISTS (SELECT 1 FROM dbo.Class WHERE ClassID = 909)
	BEGIN
		insert into dbo.Class(ClassID,Name) values (909, 'OLA')
	END

if not 1340 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1340, 909, 'OperationalLevelAgreement.Add', 'Создать', 'Операция дает возможность создавать новый объект Соглашения, но не дает возможности просмотра и изменения объекта Соглашения.')
go

if not 1341 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1341, 909, 'OperationalLevelAgreement.Update', 'Сохранить', 'Операция позволяет изменять поля объекта Соглашения через форму свойств.')
go

if not 1342 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1342, 909, 'OperationalLevelAgreement.Delete', 'Удалить', 'Операция дает возможность удалять объект Соглашения в случае отсутствия дочерних объектов.')
go

if not 1343 in (select [ID] from [dbo].[Operation])
	insert into [dbo].[Operation] ([ID], [ClassID], [Name], [OperationName], [Description])
		values (1343, 909, 'OperationalLevelAgreement.Properties', 'Открыть свойства', 'Операция позволяет просматривать поля объекта Соглашения через форму свойств, но не позволяет изменять их.')
go

update dbo.Operation set Description = 'Операция позволяет изменять поля объекта Соглашения через форму свойств.' where ID = 362;

update dbo.Operation set Description = 'Операция позволяет просматривать поля объекта Правила через форму свойств.' where ID = 364;

update dbo.Class set name = 'Правила соглашений' where ClassID = 129;

insert into [dbo].[RoleOperation] ([OperationID], [RoleID])
select t.[ID], '00000000-0000-0000-0000-000000000001'
from [dbo].[Operation] t
left join [dbo].[RoleOperation] x on x.OperationID = t.[ID] and x.[RoleID] = '00000000-0000-0000-0000-000000000001'
where (t.[ID] between 1340 and 1343) and (x.OperationID is null)
go


IF NOT EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'OperationalLevelAgreementID' AND Object_ID = Object_ID(N'dbo.Rule'))
BEGIN
    alter table [dbo].[Rule] ADD OperationalLevelAgreementID int null;
	alter table [dbo].[Rule] ADD CONSTRAINT FK_Rule_OperationalLevelAgreement FOREIGN KEY(OperationalLevelAgreementID) REFERENCES [dbo].[OperationLevelAgreement]([ID]) on delete cascade
END
go