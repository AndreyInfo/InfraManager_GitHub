IF NOT EXISTS(SELECT 1
              FROM dbo.WebFilters t
              WHERE t.[Name] = '_ALL_'
                AND t.ViewName = 'ClientsHardware')
BEGIN
    INSERT INTO dbo.WebFilters (ID, [Name], [Standart], ViewName, Others)
    VALUES (NEWID(), '_ALL_', 1, 'ClientsHardware', 1);
END
GO