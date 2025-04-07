IF NOT EXISTS(SELECT 1
              FROM information_schema.tables t
              WHERE t.table_schema = 'dbo'
                AND t.table_name = 'WorkflowActivityFormFieldSettings')
BEGIN
    CREATE TABLE dbo.WorkflowActivityFormFieldSettings (
        ID BIGINT NOT NULL IDENTITY (1,1),
        UserID UNIQUEIDENTIFIER NOT NULL,
        FieldID UNIQUEIDENTIFIER NOT NULL,
        Width INTEGER NOT NULL,
        CONSTRAINT PK_WorkflowActivityFormFieldSettings
            PRIMARY KEY (ID),
        CONSTRAINT FK_WorkflowActivityFormFieldSettings_User
            FOREIGN KEY (UserID)
            REFERENCES [dbo].[Пользователи] (IMObjID)
            ON DELETE CASCADE,
        CONSTRAINT FK_WorkflowActivityFormFieldSettings_Field
            FOREIGN KEY (FieldID)
            REFERENCES dbo.WorkflowActivityFormField (ID)
            ON DELETE CASCADE,
        CONSTRAINT UQ_WorkflowActivityFormFieldSettings
            UNIQUE (UserID, FieldID)
    );
END
GO