IF NOT EXISTS (SELECT 1
               FROM sys.default_constraints d
                   JOIN sys.all_columns c ON d.object_id = c.default_object_id
               WHERE d.type = 'D'
                 AND d.parent_object_id = OBJECT_ID('dbo.Exclusion')
                 AND c.name = 'ID')
BEGIN
  ALTER TABLE dbo.Exclusion ADD DEFAULT NEWID() FOR ID;
END
GO