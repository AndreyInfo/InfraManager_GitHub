if (EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_LifeCycleStateOperation_ByLifeCycleStateID' 
	and object_id = OBJECT_ID('LifeCycleStateOperation')))
BEGIN
	drop index LifeCycleStateOperation.UI_Name_LifeCycleStateOperation_ByLifeCycleStateID;
end;

insert into LifeCycleStateOperation(ID, Name, Sequence, CommandType, WorkOrderTemplateID, LifeCycleStateID, Icon, IconName)
select '00000000-0000-0000-0000-000000000001', Name, Sequence, CommandType, WorkOrderTemplateID, LifeCycleStateID, Icon, IconName
from LifeCycleStateOperation where ID = '00000000-0000-0000-0000-000000000000';

update RoleLifeCycleStateOperation set LifeCycleStateOperationID = '00000000-0000-0000-0000-000000000001'
where LifeCycleStateOperationID = '00000000-0000-0000-0000-000000000000';

delete from LifeCycleStateOperation where ID = '00000000-0000-0000-0000-000000000000';

if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_LifeCycleStateOperation_ByLifeCycleStateID' 
	and object_id = OBJECT_ID('LifeCycleStateOperation')))
BEGIN
	CREATE UNIQUE INDEX UI_Name_LifeCycleStateOperation_ByLifeCycleStateID on LifeCycleStateOperation (Name, LifeCycleStateID);
end;