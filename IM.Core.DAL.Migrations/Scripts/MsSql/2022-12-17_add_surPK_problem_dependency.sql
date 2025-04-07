IF NOT EXISTS (SELECT 1 FROM sys.columns AS t
               WHERE t.name = N'ID'
                 AND t.object_id = OBJECT_ID(N'dbo.ProblemDependency'))
BEGIN
    ALTER TABLE dbo.ProblemDependency
        ADD ID INT IDENTITY;
END
GO

IF 2 = ( SELECT COUNT(*)
         FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE t
         WHERE t.TABLE_SCHEMA = N'dbo'
           AND t.TABLE_NAME = N'ProblemDependency'
           AND t.CONSTRAINT_NAME = N'PK_ProblemDependency'
           AND ( t.COLUMN_NAME = N'ProblemID' OR t.COLUMN_NAME = N'ObjectID' ) )
BEGIN
    ALTER TABLE dbo.ProblemDependency
        DROP CONSTRAINT PK_ProblemDependency;
END

IF OBJECT_ID(N'PK_ProblemDependency', N'PK') IS NULL
BEGIN
    ALTER TABLE dbo.ProblemDependency
        ADD CONSTRAINT PK_ProblemDependency
        PRIMARY KEY (ID);
END
GO

IF OBJECT_ID(N'UX_ProblemDependency', N'UQ') IS NULL
BEGIN
    ALTER TABLE dbo.ProblemDependency
        ADD CONSTRAINT UX_ProblemDependency
        UNIQUE (ProblemID, ObjectID, ObjectClassID);
END
GO