DO $$
BEGIN
    IF EXISTS(SELECT 1
              FROM information_schema.tables t
              WHERE t.table_schema = 'im'
                AND t.table_name = 'massive_incident_work_order')
    THEN
        DROP TABLE im.massive_incident_work_order;
    END IF;
END;
$$