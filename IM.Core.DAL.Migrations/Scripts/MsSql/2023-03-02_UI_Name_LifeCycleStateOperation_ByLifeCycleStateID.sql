if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_LifeCycleStateOperation_ByLifeCycleStateID' 
	and object_id = OBJECT_ID('LifeCycleStateOperation')))
BEGIN
	CREATE UNIQUE INDEX UI_Name_LifeCycleStateOperation_ByLifeCycleStateID on LifeCycleStateOperation (Name, LifeCycleStateID);
end;