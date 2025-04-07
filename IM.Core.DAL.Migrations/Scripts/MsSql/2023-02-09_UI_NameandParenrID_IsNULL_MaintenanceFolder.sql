if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_ParentID_IsNull_MaintenanceFolder' and object_id = OBJECT_ID('MaintenanceFolder')))
        BEGIN
            CREATE UNIQUE INDEX UI_Name_ParentID_IsNull_MaintenanceFolder on MaintenanceFolder (Name)
			where ParentID is null;
        end;