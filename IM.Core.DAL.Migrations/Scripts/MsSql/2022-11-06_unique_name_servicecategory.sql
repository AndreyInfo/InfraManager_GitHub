if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_ServiceCategory_Name' and object_id = OBJECT_ID('ServiceCategory')))
        BEGIN
            CREATE UNIQUE INDEX UI_ServiceCategory_Name on ServiceCategory (Name);
        end;


		