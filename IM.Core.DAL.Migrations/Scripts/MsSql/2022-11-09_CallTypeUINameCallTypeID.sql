if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_CallType_Name_ParentCallTypeID' and object_id = OBJECT_ID('CallType')))
        BEGIN
            CREATE UNIQUE INDEX UI_CallType_Name_ParentCallTypeID on CallType (name, ParentCallTypeID) where removed = 0;
        end;