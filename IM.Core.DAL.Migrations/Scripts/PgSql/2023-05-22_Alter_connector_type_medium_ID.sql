DELETE FROM im.connector_kinds
    WHERE medium_id IS NULL;
ALTER TABLE IF EXISTS im.connector_kinds
    ALTER COLUMN medium_id SET NOT NULL;