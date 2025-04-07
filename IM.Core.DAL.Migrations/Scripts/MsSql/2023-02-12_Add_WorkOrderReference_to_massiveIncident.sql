
if 'MassiveIncidentWorkOrder' not in (select[TABLE_NAME] from [INFORMATION_SCHEMA].[TABLES] where[TABLE_SCHEMA] = 'dbo')
    create table [dbo].[MassiveIncidentWorkOrder] (
        [ID] bigint not null identity(1,1),
        [MassiveIncidentID] int not null,
		[WorkOrderID] uniqueidentifier not null,
		constraint PK_MassiveIncidentWorkOrder primary key clustered ([ID]),
		constraint UK_MassiveIncidentWorkOrder unique nonclustered ([MassiveIncidentID], [WorkOrderID]),
		constraint FK_MassiveIncidentWorkOrder_MassiveIncident foreign key ([MassiveIncidentID]) references [dbo].[MassiveIncident]([ID]),
		constraint FK_MassiveIncidentWorkOrder_WorkOrder foreign key ([WorkOrderID]) references [dbo].[WorkOrder]([ID]))
go


IF NOT EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'ExecutorUserID' AND Object_ID = Object_ID(N'dbo.MassiveIncident'))
BEGIN
    alter table [dbo].[MassiveIncident] ADD ExecutorUserID int null;
	alter table [dbo].[MassiveIncident] ADD CONSTRAINT fk_MassiveIncident_ExecutorUserID FOREIGN KEY(ExecutorUserID) REFERENCES [dbo].[Пользователи]([Идентификатор])
END
go

IF NOT EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'UtcDateAccomplished' AND Object_ID = Object_ID(N'dbo.MassiveIncident'))
BEGIN
	alter table [dbo].[MassiveIncident] ADD UtcDateAccomplished datetime null;
END
go

IF NOT EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'UtcDateClosed' AND Object_ID = Object_ID(N'dbo.MassiveIncident'))
BEGIN
	alter table [dbo].[MassiveIncident] ADD UtcDateClosed datetime null;
END
