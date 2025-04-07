IF NOT EXISTS(SELECT 1
              FROM dbo.WebFilters t
              WHERE t.Name = '_ALL_'
                AND t.ViewName = 'ProblemMassIncidents')
BEGIN
    INSERT INTO dbo.WebFilters (ID, [Name], Standart, ViewName, Others)
    VALUES (NEWID(), '_ALL_', 1, 'ProblemMassIncidents', 1);
END
GO

IF NOT EXISTS(SELECT 1
              FROM dbo.WebFilters t
              WHERE t.Name = '_ALL_'
                AND t.ViewName = 'MassIncidentsToAssociate')
BEGIN
    INSERT INTO dbo.WebFilters (ID, [Name], Standart, ViewName, Others)
    VALUES (NEWID(), '_ALL_', 1, 'MassIncidentsToAssociate', 1);
END
GO