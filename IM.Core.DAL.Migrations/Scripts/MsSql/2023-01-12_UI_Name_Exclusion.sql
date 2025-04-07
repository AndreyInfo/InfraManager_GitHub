if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_Exclusion' and object_id = OBJECT_ID('Exclusion')))
        BEGIN
            CREATE UNIQUE INDEX UI_Name_Exclusion on Exclusion (Name);
        end;