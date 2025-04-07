if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Role_Name' and object_id = OBJECT_ID('Role')))
        BEGIN
            CREATE UNIQUE INDEX UI_Role_Name on Role (Name);
        end;