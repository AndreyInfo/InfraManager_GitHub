IF NOT EXISTS(SELECT 1
              FROM information_schema.table_constraints t
              WHERE t.table_schema = 'dbo'
                AND t.table_name = 'FormValues'
                AND t.constraint_type = 'UNIQUE'
                AND t.constraint_name = 'UQ_FormValues')
BEGIN
    ALTER TABLE dbo.FormValues ADD UNIQUE (id, FormBuilderFormID);
END
GO