DO $$
BEGIN
    IF NOT EXISTS(SELECT 1
                  FROM im.web_filters t
                  WHERE t.name = '_ALL_'
                    AND t.view_name = 'ProblemMassIncidents')
    THEN
        INSERT INTO im.web_filters (id, name, standart, view_name, others)
        VALUES (gen_random_uuid(), '_ALL_', true, 'ProblemMassIncidents', true);
    END IF;

    IF NOT EXISTS(SELECT 1
                  FROM im.web_filters t
                  WHERE t.name = '_ALL_'
                    AND t.view_name = 'MassIncidentsToAssociate')
    THEN
        INSERT INTO im.web_filters (id, name, standart, view_name, others)
        VALUES (gen_random_uuid(), '_ALL_', true, 'MassIncidentsToAssociate', true);
    END IF;
END;
$$