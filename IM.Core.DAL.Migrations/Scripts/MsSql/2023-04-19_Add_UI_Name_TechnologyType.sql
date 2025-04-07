if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_TechnologyTypes' 
					and object_id = OBJECT_ID('[Виды технологий]')))
        BEGIN
            CREATE UNIQUE INDEX UI_Name_TechnologyTypes on [Виды технологий] (Название);
        end;