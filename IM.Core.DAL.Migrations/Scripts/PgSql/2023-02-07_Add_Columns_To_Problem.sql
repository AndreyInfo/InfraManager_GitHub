DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 -- нет initiator_id
                   FROM information_schema.columns
                   WHERE table_name = 'problem' AND column_name = 'initiator_id')
    THEN
        ALTER TABLE im.problem ADD initiator_id uuid;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_problem_initiator')
    THEN
        ALTER TABLE im.problem
            ADD CONSTRAINT fk_problem_initiator
            FOREIGN KEY (initiator_id)
            REFERENCES im.users (im_obj_id);
    END IF;


    IF NOT EXISTS (SELECT 1 -- нет queue_id
                   FROM information_schema.columns
                   WHERE table_name = 'problem' AND column_name = 'queue_id')
    THEN
        ALTER TABLE im.problem ADD queue_id uuid;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_problem_queue')
    THEN
        ALTER TABLE im.problem
            ADD CONSTRAINT fk_problem_queue
            FOREIGN KEY (queue_id)
            REFERENCES im.queue (id);
    END IF;


    IF NOT EXISTS (SELECT 1 -- нет executor_id
                   FROM information_schema.columns
                   WHERE table_name = 'problem' AND column_name = 'executor_id')
    THEN
        ALTER TABLE im.problem ADD executor_id uuid;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_problem_executor')
    THEN
        ALTER TABLE im.problem
            ADD CONSTRAINT fk_problem_executor
            FOREIGN KEY (executor_id)
            REFERENCES im.users (im_obj_id);
    END IF;


    IF NOT EXISTS (SELECT 1 -- нет service_id
                   FROM information_schema.columns
                   WHERE table_name = 'problem' AND column_name = 'service_id')
    THEN
        ALTER TABLE im.problem ADD service_id uuid;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fr_problem_service')
    THEN
        ALTER TABLE im.problem
            ADD CONSTRAINT fr_problem_service
            FOREIGN KEY (service_id)
            REFERENCES im.service (id);
    END IF;
END;
$$