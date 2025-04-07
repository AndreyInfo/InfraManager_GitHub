IF NOT EXISTS(SELECT 1 
              FROM INFORMATION_SCHEMA.TABLES t
              WHERE t.TABLE_SCHEMA = 'dbo'
                AND t.TABLE_NAME = 'ProblemToChangeRequest')
BEGIN
    CREATE TABLE dbo.ProblemToChangeRequest (
        ID int IDENTITY (1, 1),
        ProblemID uniqueidentifier NOT NULL,
        ChangeRequestID uniqueidentifier NOT NULL,
        CONSTRAINT PK_ProblemToChangeRequest
            PRIMARY KEY (ID),
        CONSTRAINT FK_ProblemToChangeRequest_Problem
            FOREIGN KEY (ProblemID)
            REFERENCES dbo.Problem (ID)
            ON DELETE CASCADE,
        CONSTRAINT FK_ProblemToChangeRequest_ChangeRequest
            FOREIGN KEY (ChangeRequestID)
            REFERENCES dbo.RFC (ID)
            ON DELETE CASCADE,
        CONSTRAINT UQ_ProblemToChangeRequest
            UNIQUE (ProblemID, ChangeRequestID)
    );
END
GO