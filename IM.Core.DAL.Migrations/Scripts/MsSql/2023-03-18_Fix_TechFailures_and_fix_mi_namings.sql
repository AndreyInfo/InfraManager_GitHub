if 'HandlingTechnicalFailures' in (select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo')
	exec sp_rename N'dbo.HandlingTechnicalFailures', 'ServiceTechnicalFailureCategory'
go

if 'TechnicalFailuresCategory' in (select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo')
	exec sp_rename N'dbo.TechnicalFailuresCategory', 'TechnicalFailureCategory'
go

if exists(select 1 from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'ServiceTechnicalFailureCategory' and CONSTRAINT_TYPE = 'PRIMARY KEY')
	alter table [dbo].[ServiceTechnicalFailureCategory] drop constraint if exists [PK_HandlingTechnicalFailures];
go
	
if exists(select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'ServiceTechnicalFailureCategory' and column_name = 'ID' and data_type = 'UNIQUEIDENTIFIER')				
begin
		exec sp_rename N'FK_HandlingTechnicalFailures_Category', N'FK_ServiceTechnicalFailureCategory_Category', N'OBJECT';
		exec sp_rename N'FK_HandlingTechnicalFailures_Group', N'FK_ServiceTechnicalFailureCategory_Group', N'OBJECT';
		exec sp_rename N'FK_HandlingTechnicalFailures_Service', N'FK_ServiceTechnicalFailureCategory_Service', N'OBJECT';
		exec sp_rename N'dbo.ServiceTechnicalFailureCategory.ID', N'IMObjID', N'COLUMN';
		exec sp_rename N'dbo.ServiceTechnicalFailureCategory.CategoryID', 'TechnicalFailureCategoryID', N'COLUMN';
end
go

if not exists(select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'ServiceTechnicalFailureCategory' and column_name = 'ID' and data_type = 'bigint')
		alter table [dbo].[ServiceTechnicalFailureCategory] add ID bigint not null IDENTITY(1,1);
go

if not exists(select 1 from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'ServiceTechnicalFailureCategory' and CONSTRAINT_TYPE = 'PRIMARY KEY')
begin
		alter table [dbo].[ServiceTechnicalFailureCategory] add constraint [PK_ServiceTechnicalFailureCategory] primary key clustered (ID);		
		alter table [dbo].[ServiceTechnicalFailureCategory] add constraint [UK_ServiceTechnicalFailureCategory] unique ([ServiceID], [TechnicalFailureCategoryID]);
		alter table [dbo].[ServiceTechnicalFailureCategory] add constraint [UK_ServiceTechnicalFailureCategory_IMObjID] unique ([IMObjID]);
end
go
	
-- fix remaining namings of mass incident tables / keys etc.
	
if 'PKMassiveIncident' in (select CONSTRAINT_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'MassIncident' and CONSTRAINT_TYPE = 'PRIMARY KEY')
begin
		exec sp_rename N'PK_MassiveIncident', N'PK_MassIncident', N'OBJECT';
    	exec sp_rename N'UK_MassiveIncident_IMObjID', N'UK_MassIncident_IMObjID', N'OBJECT';
    	exec sp_rename N'FK_MassiveIncident_Cause', N'FK_MassIncident_Cause', N'OBJECT';
    	exec sp_rename N'FK_MassiveIncident_CreatedBy', N'FK_MassIncident_CreatedBy', N'OBJECT';
    	exec sp_rename N'FK_MassiveIncident_Criticality', N'FK_MassIncident_Criticality', N'OBJECT';
    	exec sp_rename N'FK_MassiveIncident_ExecutorUserID', N'FK_MassiveIncident_Executor', N'OBJECT';
    	exec sp_rename N'FK_MassiveIncident_Group', N'FK_MassIncident_Group', N'OBJECT';
    	exec sp_rename N'FK_MassiveIncident_InformationChannel', N'FK_MassIncident_InformationChannel', N'OBJECT';
    	exec sp_rename N'FK_MassiveIncident_OwnedBy', N'FK_MassIncident_OwnedBy', N'OBJECT';
    	exec sp_rename N'FK_MassiveIncident_Priority', N'FK_MassIncident_Priority', N'OBJECT';
    	exec sp_rename N'FK_MassiveIncident_SLAID', N'FK_MassIncident_SLA', N'OBJECT';
    	exec sp_rename N'FK_MassiveIncident_Type', N'FK_MassIncident_Type', N'OBJECT';
end
go
	
if 'MassiveIncidentCall' in (select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo')
begin
	exec sp_rename N'dbo.MassiveIncidentCall.MassiveIncidentID', N'MassIncidentID', N'COLUMN';
	exec sp_rename N'PK_MassiveIncidentCall', N'PK_MassIncidentCall', N'OBJECT';
	exec sp_rename N'FK_MassiveIncidentCall_Call', N'FK_MassIncidentCall_Call', N'OBJECT';
	exec sp_rename N'FK_MassiveIncidentCall_MassiveIncident', N'FK_MassIncidentCall_MassIncident', N'OBJECT';
	exec sp_rename N'UK_MassiveIncidentCall', N'UK_MassIncidentCall', N'OBJECT';
	exec sp_rename N'dbo.MassiveIncidentCall', 'MassIncidentCall'
end
go

if 'MassiveIncidentProblem' in (select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo')
begin
	exec sp_rename N'dbo.MassiveIncidentProblem.MassiveIncidentID', N'MassIncidentID', N'COLUMN';
	exec sp_rename N'PK_MassiveIncidentProblem', N'PK_MassIncidentProblem', N'OBJECT';
	exec sp_rename N'FK_MassiveIncidentProblem_Problem', N'FK_MassIncidentProblem_Problem', N'OBJECT';
	exec sp_rename N'FK_MassiveIncidentProblem_MassiveIncident', N'FK_MassIncidentProblem_MassIncident', N'OBJECT';
	exec sp_rename N'UK_MassiveIncidentProblem', N'UK_MassIncidentProblem', N'OBJECT';
	exec sp_rename N'dbo.MassiveIncidentProblem', 'MassIncidentProblem'
end
go

if 'MassiveIncidentChangeRequest' in (select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo')
begin
	exec sp_rename N'dbo.MassiveIncidentChangeRequest.MassiveIncidentID', N'MassIncidentID', N'COLUMN';
	exec sp_rename N'PK_MassiveIncidentChangeRequest', N'PK_MassIncidentChangeRequest', N'OBJECT';
	exec sp_rename N'FK_MassiveIncidentChangeRequest_ChangeRequest', N'FK_MassIncidentChangeRequest_ChangeRequest', N'OBJECT';
	exec sp_rename N'FK_MassiveIncidentChangeRequest_MassiveIncident', N'FK_MassIncidentChangeRequest_MassIncident', N'OBJECT';
	exec sp_rename N'UK_MassiveIncidentChangeRequest', N'UK_MassIncidentChangeRequest', N'OBJECT';
	exec sp_rename N'dbo.MassiveIncidentChangeRequest', 'MassIncidentChangeRequest'
end
go

if 'MassiveIncidentWorkOrder' in (select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo')
begin
	exec sp_rename N'dbo.MassiveIncidentWorkOrder.MassiveIncidentID', N'MassIncidentID', N'COLUMN';
	exec sp_rename N'PK_MassiveIncidentWorkOrder', N'PK_MassIncidentWorkOrder', N'OBJECT';
	exec sp_rename N'FK_MassiveIncidentWorkOrder_WorkOrder', N'FK_MassIncidentWorkOrder_WorkOrder', N'OBJECT';
	exec sp_rename N'FK_MassiveIncidentWorkOrder_MassiveIncident', N'FK_MassIncidentWorkOrder_MassIncident', N'OBJECT';
	exec sp_rename N'UK_MassiveIncidentWorkOrder', N'UK_MassIncidentWorkOrder', N'OBJECT';
	exec sp_rename N'dbo.MassiveIncidentWorkOrder', 'MassIncidentWorkOrder'
end
go	
	
if 'MassiveIncidentCause' in (select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo')
begin
	exec sp_rename N'PK_MassiveIncidentCause', N'PK_MassIncidentCause', N'OBJECT';
	exec sp_rename N'dbo.MassiveIncidentCause', 'MassIncidentCause'
end
go
	
if 'MassiveIncidentInformationChannel' in (select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo')
begin
	exec sp_rename N'PK_MassiveIncidentInformationChannel', N'PK_MassIncidentInformationChannel', N'OBJECT';
	exec sp_rename N'dbo.MassiveIncidentInformationChannel', 'MassIncidentInformationChannel'
end
go
	
if 'MassiveIncidentSupervisor' in (select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo')
begin
	exec sp_rename N'dbo.MassiveIncidentSupervisor.MassiveIncidentID', N'MassIncidentID', N'COLUMN';
	exec sp_rename N'PK_MassiveIncidentSupervisor', N'PK_MassIncidentSupervisor', N'OBJECT';
	exec sp_rename N'FK_MassiveIncidentSupervisor_User', N'FK_MassIncidentSupervisor_User', N'OBJECT';
	exec sp_rename N'FK_MassiveIncidentSupervisor_MassiveIncident', N'FK_MassIncidentSupervisor_MassIncident', N'OBJECT';
	exec sp_rename N'UK_MassiveIncidentSupervisor', N'UK_MassIncidentSupervisor', N'OBJECT';
	exec sp_rename N'dbo.MassiveIncidentSupervisor', 'MassIncidentSupervisor'
end
go

if 'MassiveIncidentType' in (select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo')
begin
	exec sp_rename N'PK_MassiveIncidentType', N'PK_MassIncidentType', N'OBJECT';
	exec sp_rename N'UK_MassiveIncidentType_IMObjID', N'UK_MassIncidentType_OMObjID', N'OBJECT';
	exec sp_rename N'dbo.MassiveIncidentType', 'MassIncidentType'
end
go
