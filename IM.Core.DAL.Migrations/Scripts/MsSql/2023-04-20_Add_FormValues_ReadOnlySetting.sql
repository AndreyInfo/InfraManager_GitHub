IF NOT EXISTS(SELECT 1
              FROM information_schema.columns t
              WHERE t.table_schema = 'dbo'
                AND t.table_name = 'Values'
                AND t.column_name = 'IsReadOnly')
BEGIN
    ALTER TABLE dbo.[Values] ADD IsReadOnly BIT NOT NULL DEFAULT 0;
END
GO