if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_DashboardFolder_Name_ParentIsNull' and object_id = OBJECT_ID('DashboardFolder')))
        BEGIN
            CREATE UNIQUE INDEX UI_DashboardFolder_Name_ParentIsNull on DashboardFolder (Name)
			where ParentDashboardFolderID is null;
        end;

if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_DashboardFolder_Name_ByFolderID' and object_id = OBJECT_ID('DashboardFolder')))
        BEGIN
            CREATE UNIQUE INDEX UI_DashboardFolder_Name_ByFolderID on DashboardFolder (Name, ParentDashboardFolderID);
        end;