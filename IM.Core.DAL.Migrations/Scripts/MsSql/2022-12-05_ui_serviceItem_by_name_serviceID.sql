if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_Service_Item_Name_Into_Service' and object_id = OBJECT_ID('ServiceItem')))
        BEGIN
            CREATE UNIQUE INDEX UI_Service_Item_Name_Into_Service on ServiceItem (Name, ServiceID);
        end;