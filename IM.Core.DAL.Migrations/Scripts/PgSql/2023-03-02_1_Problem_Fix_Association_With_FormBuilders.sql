DO $$
BEGIN
    IF NOT EXISTS(SELECT 1
                  FROM information_schema.columns t
                  WHERE table_schema = 'im'
                    AND table_name = 'problem'
                    AND column_name = 'form_id')
    THEN
        ALTER TABLE im.problem ADD form_id uuid;
    END IF;

    IF EXISTS(SELECT 1
              FROM information_schema.referential_constraints t
              WHERE t.constraint_schema = 'im'
                AND t.constraint_name = 'fk_problem_form_values')
    THEN
        ALTER TABLE im.problem DROP CONSTRAINT fk_problem_form_values;
    END IF;

    UPDATE im.problem
    SET form_id = t.form_id
    FROM (SELECT p.id AS problem_id, fv.form_builder_form_id AS form_id
          FROM im.problem p JOIN im.form_values fv on p.form_values_id = fv.id) t
    WHERE id = t.problem_id;

    IF NOT EXISTS(SELECT 1
              FROM information_schema.referential_constraints t
              WHERE t.constraint_schema = 'im'
                AND t.constraint_name = 'fk_problem_form_values')
    THEN
        ALTER TABLE im.problem
            ADD CONSTRAINT fk_problem_form_values
            FOREIGN KEY (form_values_id, form_id)
            REFERENCES im.form_values (id, form_builder_form_id);
    END IF;

    IF NOT EXISTS(SELECT 1
                  FROM information_schema.check_constraints t
                  WHERE t.constraint_schema = 'im'
                    AND constraint_name = 'chk_problem_form_values')
    THEN
        ALTER TABLE im.problem ADD CONSTRAINT chk_problem_form_values CHECK ( ( form_values_id IS NOT NULL AND form_id IS NOT NULL ) OR ( form_values_id IS NULL AND form_id IS NULL ) );
    END IF;
END
$$