IF NOT EXISTS(SELECT 1
              FROM information_schema.columns t
              WHERE table_schema = 'dbo'
                AND table_name = 'RFC'
                AND column_name = 'FormID')
BEGIN
    ALTER TABLE dbo.RFC ADD FormID uniqueidentifier;
END
GO

IF EXISTS(SELECT 1
          FROM information_schema.referential_constraints t
          WHERE t.constraint_schema = 'dbo'
            AND t.constraint_name = 'FK_RFC_FormValues')
BEGIN
    ALTER TABLE dbo.RFC DROP CONSTRAINT FK_RFC_FormValues;
END
GO

UPDATE dbo.RFC
SET RFC.FormID = t.FormID
FROM (SELECT p.ID AS RFCID, fv.FormBuilderFormID AS FormID
      FROM dbo.RFC p JOIN dbo.FormValues fv on p.FormValuesID = fv.ID) t
WHERE id = t.RFCID;

IF NOT EXISTS(SELECT 1
              FROM information_schema.referential_constraints t
              WHERE t.constraint_schema = 'dbo'
                AND t.constraint_name = 'FK_RFC_FormValues')
BEGIN
    ALTER TABLE dbo.RFC
        ADD CONSTRAINT FK_RFC_FormValues
        FOREIGN KEY (FormValuesID, FormID)
        REFERENCES dbo.FormValues (ID, FormBuilderFormID);
END
GO

IF NOT EXISTS(SELECT 1
              FROM information_schema.check_constraints t
              WHERE t.constraint_schema = 'dbo'
                AND constraint_name = 'CHK_RFC_FormValues')
BEGIN
    ALTER TABLE dbo.RFC ADD CONSTRAINT CHK_RFC_FormValues CHECK ( ( RFC.FormValuesID IS NOT NULL AND RFC.FormID IS NOT NULL ) OR ( RFC.FormValuesID IS NULL AND RFC.FormID IS NULL ) );
END
GO