DELETE FROM [Виды разъемов]
    WHERE Название IN (
        SELECT Название
        FROM [Виды разъемов]
        GROUP BY Название
        HAVING COUNT(*) > 1
    )

if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_ConnectorTypes' 
			and object_id = OBJECT_ID('[Виды разъемов]')))
	BEGIN
		CREATE UNIQUE INDEX UI_Name_ConnectorTypes on [Виды разъемов] (Название);
	end;