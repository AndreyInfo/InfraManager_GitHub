if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_Quque' and object_id = OBJECT_ID('Queue')))
        BEGIN
            CREATE UNIQUE INDEX UI_Name_Quque on Queue (Name);
        end;