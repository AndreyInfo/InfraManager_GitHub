IF NOT EXISTS (SELECT 1
               FROM sys.default_constraints d
               JOIN sys.all_columns c ON d.object_id = c.default_object_id
               WHERE d.type = 'D'
                 AND d.parent_object_id = OBJECT_ID('dbo.WorkOrder')
                 AND c.name = 'Number')
BEGIN
    ALTER TABLE dbo.WorkOrder ADD DEFAULT NEXT VALUE FOR WorkOrderNumber FOR [Number];
END
GO