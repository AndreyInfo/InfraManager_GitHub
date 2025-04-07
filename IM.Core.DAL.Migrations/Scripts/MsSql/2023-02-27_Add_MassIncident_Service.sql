if 'ServiceID' not in (select [COLUMN_NAME] FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'MassIncident')
begin
    alter table [dbo].[MassIncident] add [ServiceID] uniqueidentifier NULL
	alter table [dbo].[MassIncident] add constraint [FK_MassIncident_Service] foreign key ([ServiceID]) references [Service] ([ID])
end
go

if exists(select 1 from [dbo].[MassIncident] where [ServiceID] IS NULL)
begin
	update [dbo].[MassIncident] set [ServiceID] = (select x.[ServiceID] from [dbo].[CallService] x where x.[ID] = [CallServiceID])
	alter table [dbo].[MassIncident] alter column [ServiceID] uniqueidentifier NOT NULL
end
go

if 'CallServiceID' in (select [COLUMN_NAME] FROM INFORMATION_SCHEMA.COLUMNS WHERE [TABLE_SCHEMA] = 'dbo' AND [TABLE_NAME] = 'MassIncident')
begin
	alter table [dbo].[MassIncident] drop constraint [FK_MassiveIncident_CallServiceID]
	alter table [dbo].[MassIncident] drop column [CallServiceID]
end
go

if 'MassiveIncidentAffectedCallService' in (select [TABLE_NAME] from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'MassiveIncidentAffectedCallService')
begin
    drop table [dbo].[MassiveIncidentAffectedCallService];
end
go

if 'MassIncidentAffectedService' not in (select [TABLE_NAME] from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo')
begin
    create table [dbo].[MassIncidentAffectedService] (
        ID bigint IDENTITY constraint [PK_MassIncidentService] PRIMARY KEY,
        MassIncidentID int NOT NULL constraint FK_MassIncidentService_MassIncident REFERENCES [dbo].[MassIncident] ([ID]),
        ServiceID uniqueidentifier NOT NULL constraint FK_MassIncidentService_Service REFERENCES [dbo].[Service],
        constraint UK_MassIncidentService UNIQUE (MassIncidentID, ServiceID));
end
go

if 'AllMassiveIncidentsList' not in (select [ViewName] from [dbo].[WebFilters] where [Name] = '_ALL_')
	delete from [dbo].[WebFilterUsing] where [FilterID] in (select [ID] from [dbo].[WebFilters] where [ViewName] = 'AllMassiveIncidentsList')
	delete from [dbo].[WebFilters] where [ViewName] = 'AllMassiveIncidentsList'
go

if 'AllMassIncidentsList' not in (select [ViewName] from [dbo].[WebFilters] where [Name] = '_ALL_')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_ALL_', 1, 'AllMassIncidentsList', 1)
go