DELETE FROM [Unit]
	WHERE Name IN (
		SELECT Name
		FROM [Unit]
		GROUP BY Name
		HAVING COUNT(*) > 1
	)

if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_Unit' 
	and object_id = OBJECT_ID('[Unit]')))
        BEGIN
            CREATE UNIQUE INDEX UI_Name_Unit on [Unit] (Name);
        end;