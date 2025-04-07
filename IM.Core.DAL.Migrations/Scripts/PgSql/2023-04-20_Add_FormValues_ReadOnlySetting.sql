DO $$
BEGIN
    IF NOT EXISTS(SELECT 1
        FROM information_schema.columns t
        WHERE t.table_schema = 'im'
          AND t.table_name = 'values'
          AND t.column_name = 'is_read_only')
    THEN
        ALTER TABLE im.values ADD COLUMN is_read_only bool NOT NULL DEFAULT (false);
    END IF;
END;
$$