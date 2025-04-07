IF EXISTS(SELECT 1
          FROM sys.indexes t
          WHERE t.[Name] = 'UI_Dashboard_Name_ByFolderID'
            AND t.object_id = OBJECT_ID('Dashboard'))
BEGIN
    DROP INDEX UI_Dashboard_Name_ByFolderID ON dbo.Dashboard;
END
GO

IF NOT EXISTS(SELECT 1
          FROM sys.indexes t
          WHERE t.[Name] = 'UI_Dashboard_Name_ByFolderID'
            AND t.object_id = OBJECT_ID('Dashboard'))
BEGIN
    CREATE UNIQUE INDEX UI_Dashboard_Name_ByFolderID ON dbo.Dashboard (Name, DashboardFolderID);
END
GO