IF NOT EXISTS(SELECT 1
    FROM information_schema.columns t
    WHERE t.table_schema = 'dbo'
      AND t.table_name = 'Values'
    AND t.column_name = 'RowNumber')
BEGIN
    ALTER TABLE [dbo].[Values] ADD [RowNumber] int NOT NULL DEFAULT 0;
END
GO