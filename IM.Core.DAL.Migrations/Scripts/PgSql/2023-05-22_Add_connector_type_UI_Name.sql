DO $$
BEGIN

    DELETE FROM connector_kinds
        WHERE name IN (
            SELECT name
            FROM connector_kinds
            GROUP BY name
            HAVING COUNT(*) > 1
        );

    CREATE UNIQUE INDEX 
        if not exists ui_name_connector_kinds
        on connector_kinds(name);
END $$