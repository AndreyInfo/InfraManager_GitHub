CREATE TABLE IF NOT EXISTS im.schedule_task
(
    id uuid NOT NULL,
    name character varying(250) COLLATE pg_catalog."default" NOT NULL,
    task_type integer NOT NULL,
    task_setting_id uuid,
    note character varying(1000) COLLATE pg_catalog."default",
    is_enabled boolean NOT NULL,
    use_account boolean NOT NULL,
    task_state integer NOT NULL,
    next_run_at timestamp without time zone,
    finish_run_at timestamp without time zone,
    task_setting_name character varying(250) COLLATE pg_catalog."default",
    credential_id integer,
    CONSTRAINT schedule_task_pkey PRIMARY KEY (id),
    CONSTRAINT fk_schedule_task_user_account FOREIGN KEY (credential_id)
        REFERENCES im.user_account (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

