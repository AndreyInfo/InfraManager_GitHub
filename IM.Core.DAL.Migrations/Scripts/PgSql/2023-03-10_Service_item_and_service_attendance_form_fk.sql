ALTER TABLE IF EXISTS im.service_item
    ADD CONSTRAINT fk_service_item_form_id FOREIGN KEY (form_id)
    REFERENCES im.form_builder_form (id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;

ALTER TABLE IF EXISTS im.service_attendance
    ADD CONSTRAINT fk_service_attendance_form_id FOREIGN KEY (form_id)
    REFERENCES im.form_builder_form (id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;