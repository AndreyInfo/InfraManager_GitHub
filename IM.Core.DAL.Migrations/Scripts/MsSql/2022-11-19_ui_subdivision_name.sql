if (NOT EXISTS(SELECT 0 FROM sys.indexes where name = 'UI_Subdivision_organization_ParentSubdivision_Name' and object_id = OBJECT_ID('Подразделение')))
        BEGIN
            CREATE UNIQUE INDEX UI_Subdivision_organization_ParentSubdivision_Name on Подразделение (Название, [ИД организации] ,[ИД подразделения]);
        end;