IF COL_LENGTH('CallType','WorkflowSchemeId') IS NULL
BEGIN
	ALTER TABLE CallType ADD WorkflowSchemeId uniqueidentifier NULL
END

IF COL_LENGTH('CallType','IconName') IS NULL
BEGIN
	ALTER TABLE CallType ADD IconName nvarchar(200) NULL
END

IF COL_LENGTH('CallType','IsFixed') IS NULL
BEGIN
	ALTER TABLE CallType ADD IsFixed bit default 0 not NULL
END

IF COL_LENGTH('ServiceItem','FormID') IS NULL
BEGIN
	ALTER TABLE ServiceItem ADD FormID uniqueidentifier NULL
END

IF COL_LENGTH('ServiceAttendance','FormID') IS NULL
BEGIN
	ALTER TABLE ServiceAttendance ADD FormID uniqueidentifier NULL
END

IF COL_LENGTH('ProblemType','FormID') IS NULL
BEGIN
	ALTER TABLE ProblemType ADD FormID uniqueidentifier NULL
END

IF COL_LENGTH('ProblemType','ImageName') IS NULL
BEGIN
	ALTER TABLE ProblemType ADD ImageName nvarchar(200) NULL
END

IF COL_LENGTH('WorkOrderTemplate','FormID') IS NULL
BEGIN
	ALTER TABLE WorkOrderTemplate ADD FormID uniqueidentifier NULL
END

IF COL_LENGTH('SLA','FormID') IS NULL
BEGIN
	ALTER TABLE SLA ADD FormID uniqueidentifier NULL
END

IF COL_LENGTH('LifeCycleStateOperation','IconName') IS NULL
BEGIN
	ALTER TABLE LifeCycleStateOperation ADD IconName nvarchar(200) NULL
END

if ((SELECT object_definition(default_object_id) AS definition FROM sys.columns WHERE name ='ID' AND object_id = object_id('dbo.Role')) is null)
begin
	alter table Role add default NEWID() for ID
end;
