IF NOT EXISTS (SELECT 1
               FROM sys.default_constraints d
               JOIN sys.all_columns c ON d.object_id = c.default_object_id
               WHERE d.type = 'D'
                 AND d.parent_object_id = OBJECT_ID('dbo.EntityEvent')
                 AND c.name = 'Order')
BEGIN
    ALTER TABLE dbo.EntityEvent ADD DEFAULT NEXT VALUE FOR ExternalEventNumber FOR [Order];
END
GO