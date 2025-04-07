if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_Solution' and object_id = OBJECT_ID('Solution')))
BEGIN
	CREATE UNIQUE INDEX UI_Name_Solution on Solution (Name);
END;