CREATE TABLE IF NOT EXISTS im.handling_technical_failures
(
    id uuid DEFAULT (gen_random_uuid()),
    service_id uuid,
    category_id integer,
    group_id uuid,
    PRIMARY KEY (id),
    CONSTRAINT fk_handling_technical_failures_service FOREIGN KEY (service_id)
        REFERENCES im.service (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
        NOT VALID,
    CONSTRAINT fk_handling_technical_failures_category FOREIGN KEY (category_id)
        REFERENCES im.technical_failures_category (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
        NOT VALID,
    CONSTRAINT fk_handling_technical_failures_group FOREIGN KEY (group_id)
        REFERENCES im.queue (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
        NOT VALID
);

CREATE UNIQUE INDEX 
    if not exists ui_handling_technical_failures_servcie_category
    on handling_technical_failures(service_id, category_id);