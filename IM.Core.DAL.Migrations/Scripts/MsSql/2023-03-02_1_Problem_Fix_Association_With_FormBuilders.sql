IF NOT EXISTS(SELECT 1
              FROM information_schema.columns t
              WHERE table_schema = 'dbo'
                AND table_name = 'Problem'
                AND column_name = 'FormID')
BEGIN
    ALTER TABLE dbo.Problem ADD FormID uniqueidentifier;
END
GO

IF EXISTS(SELECT 1
          FROM information_schema.referential_constraints t
          WHERE t.constraint_schema = 'dbo'
            AND t.constraint_name = 'FK_Problem_FormValues')
BEGIN
    ALTER TABLE dbo.Problem DROP CONSTRAINT FK_Problem_FormValues;
END;
GO

UPDATE dbo.Problem
SET FormID = t.FormID
FROM (SELECT p.ID AS ProblemID, fv.FormBuilderFormID AS FormID
      FROM dbo.Problem p JOIN dbo.FormValues fv on p.FormValuesID = fv.ID) t
WHERE ID = t.ProblemID;

IF NOT EXISTS(SELECT 1
              FROM information_schema.referential_constraints t
              WHERE t.constraint_schema = 'dbo'
                AND t.constraint_name = 'FK_Problem_FormValues')
BEGIN
    ALTER TABLE dbo.Problem
        ADD CONSTRAINT FK_Problem_FormValues
        FOREIGN KEY (FormValuesID, FormID)
        REFERENCES dbo.FormValues (ID, FormBuilderFormID);
END
GO

IF NOT EXISTS(SELECT 1
              FROM information_schema.check_constraints t
              WHERE t.constraint_schema = 'dbo'
                AND constraint_name = 'CHK_Problem_FormValues')
BEGIN
    ALTER TABLE dbo.Problem ADD CONSTRAINT CHK_Problem_FormValues CHECK ( ( Problem.FormValuesID IS NOT NULL AND Problem.FormID IS NOT NULL ) OR ( Problem.FormValuesID IS NULL AND Problem.FormID IS NULL ) );
END
GO