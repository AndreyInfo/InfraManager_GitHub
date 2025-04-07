if '_MI_MY_NOT_STARTED_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_MY_NOT_STARTED_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_MY_OPENED_OVERUDE_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_MY_OPENED_OVERUDE_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_UNASSIGNED_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_UNASSIGNED_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_OTHERS_OVERDUE_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_OTHERS_OVERDUE_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_MY_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_MY_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_MY_IN_WORK_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_MY_IN_WORK_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_MY_COMPLETED_CONFIRM_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_MY_COMPLETED_CONFIRM_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_MY_CLOSED_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_MY_CLOSED_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_NOT_MY_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_NOT_MY_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_NOT_MY_INWORK_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_NOT_MY_INWORK_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_NOT_MY_COMPLETED_CONFIRM_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_NOT_MY_COMPLETED_CONFIRM_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_NOT_MY_CLOSED_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_NOT_MY_CLOSED_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_OWNED_OPENED_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_OWNED_OPENED_', 1, 'AllMassIncidentsList', 1)
go
if '_MI_EXECUTED_OPENED_' NOT IN (select [Name] from [dbo].[WebFilters] where [ViewName] = 'AllMassIncidentsList')
	insert into [dbo].[WebFilters] ([ID], [Name], [Standart], [ViewName], [Others]) values (NEWID(), '_MI_EXECUTED_OPENED_', 1, 'AllMassIncidentsList', 1)
go

drop index if exists IX_MassIncident_CreatedBy on [dbo].[MassIncident]
go
create index IX_MassIncident_CreatedBy on [dbo].[MassIncident] ([CreatedByUserID])
go

drop index if exists IX_MassIncident_OwnedBy on [dbo].[MassIncident]
go
create index IX_MassIncident_OwnedBy on [dbo].[MassIncident] ([OwnedByUserID])
go

drop index if exists IX_MassIncident_Type on [dbo].[MassIncident]
go
create index IX_MassIncident_Type on [dbo].[MassIncident] ([TypeID])
go

drop index if exists IX_MassIncident_Name on [dbo].[MassIncident]
go
create index IX_MassIncident_Name on [dbo].[MassIncident] ([Name])
go

drop index if exists IX_MassIncident_Description on [dbo].[MassIncident]
go
create index IX_MassIncident_Description on [dbo].[MassIncident] ([DescriptionPlain])
go

drop index if exists IX_MassIncident_Solution on [dbo].[MassIncident]
go
create index IX_MassIncident_Solution on [dbo].[MassIncident] ([SolutionPlain])
go

drop index if exists IX_MassIncident_ServiceID on [dbo].[MassIncident]
go
create index IX_MassIncident_ServiceID on [dbo].[MassIncident] ([ServiceID])
go

drop index if exists IX_MassIncident_State on [dbo].[MassIncident]
go
create index IX_MassIncident_State on [dbo].[MassIncident] ([EntityStateID])
go

drop index if exists IX_MassIncident_CreatedAt on [dbo].[MassIncident]
go
create index IX_MassIncident_CreatedAt on [dbo].[MassIncident] ([UtcCreatedAt])
go

drop index if exists IX_MassIncident_ModifiedAt on [dbo].[MassIncident]
go
create index IX_MassIncident_ModifiedAt on [dbo].[MassIncident] ([UtcLastModifiedAt])
go

drop index if exists IX_MassIncident_OpenedAt on [dbo].[MassIncident]
go
create index IX_MassIncident_OpenedAt on [dbo].[MassIncident] ([UtcOpenedAt])
go

drop index if exists IX_MassIncident_RegisteredAt on [dbo].[MassIncident]
go
create index IX_MassIncident_RegisteredAt on [dbo].[MassIncident] ([UtcRegisteredAt])
go

drop index if exists IX_MassIncident_CloseUntil on [dbo].[MassIncident]
go
create index IX_MassIncident_CloseUntil on [dbo].[MassIncident] ([UtcCloseUntil])
go

drop index if exists IX_MassIncident_ExecutedByGroup on [dbo].[MassIncident]
go
create index IX_MassIncident_ExecutedByGroup on [dbo].[MassIncident] ([ExecutedByGroupID])
go

drop index if exists IX_MassIncident_OLA on [dbo].[MassIncident]
go
create index IX_MassIncident_OLA on [dbo].[MassIncident] ([OLAID])
go
