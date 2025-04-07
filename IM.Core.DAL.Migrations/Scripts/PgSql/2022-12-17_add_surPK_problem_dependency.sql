DO
$$
BEGIN

    IF NOT EXISTS (SELECT 1
                   FROM information_schema.columns t
                   WHERE t.table_schema = 'im'
                     AND t.table_name = 'problem_dependency'
                     AND column_name = 'id')
    THEN
        ALTER TABLE IF EXISTS im.problem_dependency
            ADD COLUMN IF NOT EXISTS id INT
            GENERATED ALWAYS AS IDENTITY;
    END IF;

    IF 2 = (SELECT COUNT(*)
            FROM pg_index i
            JOIN pg_attribute a ON a.attrelid = i.indrelid AND a.attnum = ANY(i.indkey)
            WHERE i.indisprimary AND NOT a.attisdropped
              AND i.indrelid = 'im.problem_dependency'::regclass
              AND ( a.attname = 'problem_id' OR a.attname = 'object_id' ))
    THEN
        ALTER TABLE IF EXISTS im.problem_dependency
            DROP CONSTRAINT pk_problem_dependency;
    END IF;

    IF NOT EXISTS (SELECT 1
                   FROM pg_index i
                   WHERE i.indisprimary
                     AND i.indrelid = 'im.problem_dependency'::regclass)
    THEN
        ALTER TABLE IF EXISTS im.problem_dependency
            ADD CONSTRAINT pk_problem_dependency PRIMARY KEY (id);
    END IF;

    IF NOT EXISTS (SELECT 1
                   FROM pg_index i
                   WHERE i.indisunique
                     AND NOT i.indisprimary
                     AND i.indrelid = 'im.problem_dependency'::regclass)
    THEN
        ALTER TABLE IF EXISTS im.problem_dependency
            ADD CONSTRAINT ux_problem_dependency
            UNIQUE (problem_id, object_id, object_class_id);
    END IF;

END;
$$;