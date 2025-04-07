if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_AD_Name_Configuration' and object_id = OBJECT_ID('UIADConfiguration')))
        BEGIN
            CREATE UNIQUE INDEX UI_AD_Name_Configuration on  UIADConfiguration (Name);
        end;