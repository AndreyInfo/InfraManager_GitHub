CREATE TABLE IF NOT EXISTS im.schedule
(
    id uuid NOT NULL,
    "interval" integer,
    schedule_type integer NOT NULL,
    schedule_task_id uuid NOT NULL,
    start_at timestamp without time zone,
    finish_at timestamp without time zone,
    days_of_week character varying(100) COLLATE pg_catalog."default",
    months character varying(100) COLLATE pg_catalog."default",
    CONSTRAINT task_schedule_pkey PRIMARY KEY (id),
    CONSTRAINT task_schedule_schedule_task FOREIGN KEY (schedule_task_id)
        REFERENCES im.schedule_task (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
        NOT VALID
)

