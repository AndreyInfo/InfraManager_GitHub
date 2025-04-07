if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_KBArticleStatus_Name' and object_id = OBJECT_ID('KBArticleStatus')))
        BEGIN
            CREATE UNIQUE INDEX UI_KBArticleStatus_Name on KBArticleStatus (Name);
        end;