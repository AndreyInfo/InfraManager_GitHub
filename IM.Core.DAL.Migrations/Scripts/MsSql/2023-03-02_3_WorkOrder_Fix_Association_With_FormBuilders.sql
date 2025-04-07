IF NOT EXISTS(SELECT 1
              FROM information_schema.columns t
              WHERE table_schema = 'dbo'
                AND table_name = 'WorkOrder'
                AND column_name = 'FormID')
BEGIN
    ALTER TABLE dbo.WorkOrder ADD FormID uniqueidentifier;
END
GO

IF EXISTS(SELECT 1
          FROM information_schema.referential_constraints t
          WHERE t.constraint_schema = 'dbo'
            AND t.constraint_name = 'FK_WorkOrder_FormValues')
BEGIN
    ALTER TABLE dbo.WorkOrder DROP CONSTRAINT FK_WorkOrder_FormValues;
END
GO

UPDATE dbo.WorkOrder
SET FormID = t.FormID
FROM (SELECT p.id AS WorkOrderID, fv.FormBuilderFormID AS FormID
      FROM dbo.WorkOrder p JOIN dbo.FormValues fv on p.FormValuesID = fv.ID) t
WHERE ID = t.WorkOrderID;

IF NOT EXISTS(SELECT 1
              FROM information_schema.referential_constraints t
              WHERE t.constraint_schema = 'dbo'
                AND t.constraint_name = 'FK_WorkOrder_FormValues')
BEGIN
    ALTER TABLE dbo.WorkOrder
        ADD CONSTRAINT FK_WorkOrder_FormValues
        FOREIGN KEY (FormValuesID, FormID)
        REFERENCES dbo.FormValues (ID, FormBuilderFormID);
END
GO

IF NOT EXISTS(SELECT 1
              FROM information_schema.check_constraints t
              WHERE t.constraint_schema = 'dbo'
                AND constraint_name = 'CHK_WorkOrder_FormValues')
BEGIN
    ALTER TABLE dbo.WorkOrder ADD CONSTRAINT CHK_WorkOrder_FormValues CHECK ( ( WorkOrder.FormValuesID IS NOT NULL AND WorkOrder.FormID IS NOT NULL ) OR ( WorkOrder.FormValuesID IS NULL AND WorkOrder.FormID IS NULL ) );
END
GO