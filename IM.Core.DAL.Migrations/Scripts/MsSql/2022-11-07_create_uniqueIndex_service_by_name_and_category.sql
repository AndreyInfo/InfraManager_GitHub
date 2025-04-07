    if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_ServiceName_In_ServiceCategory' and object_id = OBJECT_ID('Service')))
        BEGIN
            CREATE UNIQUE INDEX UI_ServiceName_In_ServiceCategory on service(name, ServiceCategoryId);
        end;
