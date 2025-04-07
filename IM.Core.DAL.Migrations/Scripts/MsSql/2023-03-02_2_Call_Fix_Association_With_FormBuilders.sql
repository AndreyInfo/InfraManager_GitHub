IF NOT EXISTS(SELECT 1
              FROM information_schema.columns t
              WHERE table_schema = 'dbo'
                AND table_name = 'Call'
                AND column_name = 'FormID')
BEGIN
    ALTER TABLE dbo.Call ADD FormID uniqueidentifier;
END
GO

IF EXISTS(SELECT 1
          FROM information_schema.referential_constraints t
          WHERE t.constraint_schema = 'dbo'
            AND t.constraint_name = 'FK_Call_FormValues')
BEGIN
    ALTER TABLE dbo.Call DROP CONSTRAINT FK_Call_FormValues;
END
GO

UPDATE dbo.Call
SET Call.FormID = t.FormID
FROM (SELECT p.ID AS CallID, fv.FormBuilderFormID AS FormID
      FROM dbo.Call p JOIN dbo.FormValues fv on p.FormValuesID = fv.ID) t
WHERE id = t.CallID;

IF NOT EXISTS(SELECT 1
              FROM information_schema.referential_constraints t
              WHERE t.constraint_schema = 'dbo'
                AND t.constraint_name = 'FK_Call_FormValues')
BEGIN
    ALTER TABLE dbo.Call
        ADD CONSTRAINT FK_Call_FormValues
        FOREIGN KEY (FormValuesID, FormID)
        REFERENCES dbo.FormValues (ID, FormBuilderFormID);
END
GO

IF NOT EXISTS(SELECT 1
              FROM information_schema.check_constraints t
              WHERE t.constraint_schema = 'dbo'
                AND constraint_name = 'CHK_Call_FormValues')
BEGIN
    ALTER TABLE dbo.Call ADD CONSTRAINT CHK_Call_FormValues CHECK ( ( Call.FormValuesID IS NOT NULL AND Call.FormID IS NOT NULL ) OR ( Call.FormValuesID IS NULL AND Call.FormID IS NULL ) );
END
GO