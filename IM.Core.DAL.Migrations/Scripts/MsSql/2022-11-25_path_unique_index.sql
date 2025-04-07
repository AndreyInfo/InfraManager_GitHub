if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_AD_Path_Settings_Path' and object_id = OBJECT_ID('UIADPath')))
        BEGIN
            CREATE UNIQUE INDEX UI_AD_Path_Settings_Path on  UIADPath (ADSettingID,  Path);
        end;