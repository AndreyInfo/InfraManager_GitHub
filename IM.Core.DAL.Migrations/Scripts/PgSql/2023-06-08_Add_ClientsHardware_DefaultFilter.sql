DO $$
BEGIN
    IF NOT EXISTS(SELECT 1
                  FROM im.web_filters t
                  WHERE t.name = '_ALL_'
                    AND t.view_name = 'ClientsHardware')
    THEN
        INSERT INTO im.web_filters (id, name, standart, view_name, others)
        VALUES (gen_random_uuid(), '_ALL_', true, 'ClientsHardware', true);
    END IF;
END;
$$