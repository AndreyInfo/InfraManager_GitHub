if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Name_Notification' and object_id = OBJECT_ID('Notification')))
        BEGIN
            CREATE UNIQUE INDEX UI_Name_Notification on Notification (Name);
        end;