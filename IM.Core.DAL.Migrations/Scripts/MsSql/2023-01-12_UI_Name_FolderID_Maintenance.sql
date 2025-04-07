if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_FolderID_Maintenance' and object_id = OBJECT_ID('Maintenance')))
BEGIN
	CREATE UNIQUE INDEX UI_Name_FolderID_Maintenance on Maintenance (Name, MaintenanceFolderID);
end;