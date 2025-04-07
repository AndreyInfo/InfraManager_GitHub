IF NOT EXISTS (SELECT 1
	FROM sys.objects
	WHERE name ='DF_LifeCycleStateID')
BEGIN 
	ALTER TABLE [LifeCycleState]
		ADD CONSTRAINT DF_LifeCycleStateID DEFAULT NEWID() for ID;
END;