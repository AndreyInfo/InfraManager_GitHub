if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_LifeCycleState_ByLifeCycleID' 
								and object_id = OBJECT_ID('LifeCycleState')))
BEGIN
	CREATE UNIQUE INDEX UI_Name_LifeCycleState_ByLifeCycleID on LifeCycleState (Name, LifeCycleID);
end;