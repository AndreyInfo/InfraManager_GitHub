Update LifeCycle
SET Fixed = 1
WHERE id = '00000000-0000-0000-0000-000000000020';



if not exists (SELECT 1
				FROM sys.tables t
				JOIN sys.default_constraints f ON t.object_id = f.parent_object_id
				WHERE t.name = 'LifeCycleState')
	BEGIN 
		ALTER TABLE LifeCycleState
		ADD CONSTRAINT DF_LifeCycleStateID 
		DEFAULT NEWID() for ID;
	END