DO $$
BEGIN
    IF NOT EXISTS(SELECT 1
                  FROM information_schema.table_constraints t
                  WHERE t.table_schema = 'im'
                    AND t.table_name = 'form_values'
                    AND t.constraint_type = 'UNIQUE'
                    AND t.constraint_name = 'uq_form_values')
    THEN
        ALTER TABLE im.form_values ADD UNIQUE (id, form_builder_form_id);
    END IF;
END;
$$