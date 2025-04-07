if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_User_Activity_Type_User' and object_id = OBJECT_ID('UserActivityTypeReference')))
        BEGIN
            CREATE UNIQUE INDEX UI_User_Activity_Type_User on UserActivityTypeReference (ObjectID, UserActivityTypeID);
        end;