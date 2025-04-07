DO $$
BEGIN
    IF NOT EXISTS(SELECT 1
        FROM information_schema.columns t
        WHERE t.table_schema = 'im'
          AND t.table_name = 'values'
          AND t.column_name = 'row_number')
    THEN
        ALTER TABLE im.values ADD row_number int NOT NULL DEFAULT 0;
    END IF;
END;
$$