IF NOT EXISTS(SELECT 1
              FROM information_schema.referential_constraints t
              WHERE t.constraint_schema = 'dbo'
                AND t.constraint_name = 'FK_MassIncidentType_Form')
BEGIN
    ALTER TABLE dbo.MassiveIncidentType
        ADD CONSTRAINT FK_MassIncidentType_Form
        FOREIGN KEY (FormID)
        REFERENCES dbo.WorkflowActivityForm (ID); 
END
GO

IF NOT EXISTS(SELECT 1
              FROM information_schema.columns t
              WHERE t.table_schema = 'dbo'
                AND t.table_name = 'MassIncident'
                AND t.column_name = 'FormValuesID')
BEGIN
    ALTER TABLE dbo.MassIncident ADD FormValuesID bigint;
END
GO

IF NOT EXISTS(SELECT 1
              FROM information_schema.columns t
              WHERE t.table_schema = 'dbo'
                AND t.table_name = 'MassIncident'
                AND t.column_name = 'FormID')
BEGIN
    ALTER TABLE dbo.MassIncident ADD FormID uniqueidentifier;
END
GO

IF NOT EXISTS(SELECT 1
              FROM information_schema.referential_constraints t
              WHERE t.constraint_schema = 'dbo'
                AND t.constraint_name = 'FK_MassIncident_FormValues')
BEGIN
    ALTER TABLE dbo.MassIncident
        ADD CONSTRAINT FK_MassIncident_FormValues
        FOREIGN KEY (FormValuesID, FormID)
        REFERENCES dbo.FormValues (ID, FormBuilderFormID);
END
GO

IF NOT EXISTS(SELECT 1
              FROM information_schema.check_constraints t
              WHERE t.constraint_schema = 'dbo'
                AND constraint_name = 'CHK_MassIncident_FormValues')
BEGIN
ALTER TABLE dbo.MassIncident ADD CONSTRAINT CHK_MassIncident_FormValues CHECK ( ( FormValuesID IS NOT NULL AND FormID IS NOT NULL ) OR ( FormValuesID IS NULL AND FormID IS NULL ) );
END
GO