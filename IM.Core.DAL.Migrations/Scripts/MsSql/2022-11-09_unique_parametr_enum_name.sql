if (NOT EXISTS(SELECT TOP 1 0 FROM sys.indexes where name = 'UI_ParameterEnum_Name' and object_id = OBJECT_ID('[ParameterEnum]')))
        BEGIN
            CREATE UNIQUE INDEX UI_ParameterEnum_Name on [ParameterEnum] ([Name]);
        end;