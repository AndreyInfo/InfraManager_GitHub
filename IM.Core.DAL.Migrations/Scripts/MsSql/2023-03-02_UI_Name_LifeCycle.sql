if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_LifeCycle' 
							and object_id = OBJECT_ID('LifeCycle')))
        BEGIN
            CREATE UNIQUE INDEX UI_Name_LifeCycle on LifeCycle (Name)
			where Removed = 0;
        end;