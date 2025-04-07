DO $$
BEGIN
    IF NOT EXISTS(SELECT 1
                  FROM information_schema.referential_constraints t
                  WHERE t.constraint_schema = 'im'
                    AND t.constraint_name = 'fk_mass_incident_type_form')
    THEN
        ALTER TABLE im.massive_incident_type
            ADD CONSTRAINT fk_mass_incident_type_form
            FOREIGN KEY (form_id)
            REFERENCES im.form_builder_form (id); 
    END IF;
    
    IF NOT EXISTS(SELECT 1
                  FROM information_schema.columns t
                  WHERE t.table_schema = 'im'
                    AND t.table_name = 'mass_incident'
                    AND t.column_name = 'form_values_id')
    THEN
        ALTER TABLE im.mass_incident ADD form_values_id bigint;
    END IF;

    IF NOT EXISTS(SELECT 1
                  FROM information_schema.columns t
                  WHERE t.table_schema = 'im'
                    AND t.table_name = 'mass_incident'
                    AND t.column_name = 'form_id')
    THEN
        ALTER TABLE im.mass_incident ADD form_id uuid;
    END IF;

    IF NOT EXISTS(SELECT 1
                  FROM information_schema.referential_constraints t
                  WHERE t.constraint_schema = 'im'
                    AND t.constraint_name = 'fk_mass_incident_form_values')
    THEN
        ALTER TABLE im.mass_incident
            ADD CONSTRAINT fk_mass_incident_form_values
            FOREIGN KEY (form_values_id, form_id)
            REFERENCES im.form_values (id, form_builder_form_id);
    END IF;

    IF NOT EXISTS(SELECT 1
                  FROM information_schema.check_constraints t
                  WHERE t.constraint_schema = 'im'
                    AND constraint_name = 'chk_mass_incident_form_values')
    THEN
        ALTER TABLE im.mass_incident ADD CONSTRAINT chk_mass_incident_form_values CHECK ( ( form_values_id IS NOT NULL AND form_id IS NOT NULL ) OR ( form_values_id IS NULL AND form_id IS NULL ) );
    END IF;
    
    IF EXISTS(SELECT 1
              FROM information_schema.sequences t
              WHERE t.sequence_schema = 'im'
                AND t.sequence_name = 'massive_incident_work_order_id')
    THEN
        DROP SEQUENCE im.massive_incident_work_order_id;
    END IF;
END;
$$