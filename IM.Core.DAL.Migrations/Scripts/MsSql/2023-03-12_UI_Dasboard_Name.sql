if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Dashboard_Name_ByFolderID' and object_id = OBJECT_ID('Dashboard')))
BEGIN
	CREATE UNIQUE INDEX UI_Dashboard_Name_ByFolderID on Dashboard (Name);
end;