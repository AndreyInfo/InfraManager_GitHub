DO $$
BEGIN
    CREATE SEQUENCE IF NOT EXISTS problem_to_change_request_id_seq START 1 INCREMENT 1;

    CREATE TABLE IF NOT EXISTS im.problem_to_change_request (
        id int DEFAULT (nextval('problem_to_change_request_id_seq')),
        problem_id uuid NOT NULL,
        change_request_id uuid NOT NULL,
        CONSTRAINT pk_problem_to_change_request
            PRIMARY KEY (id),
        CONSTRAINT fk_problem_to_change_request_problem
            FOREIGN KEY (problem_id)
            REFERENCES im.problem (id)
            ON DELETE CASCADE,
        CONSTRAINT fk_problem_to_change_request_change_request
            FOREIGN KEY (change_request_id)
            REFERENCES im.rfc (id)
            ON DELETE CASCADE,
        CONSTRAINT uq_problem_to_change_request
            UNIQUE (problem_id, change_request_id)
    );
END;
$$