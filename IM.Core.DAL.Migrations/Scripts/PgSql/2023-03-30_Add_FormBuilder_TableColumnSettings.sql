DO $$
BEGIN
    CREATE SEQUENCE IF NOT EXISTS form_builder_form_tabs_fields_settings_id START 1 INCREMENT 1;

    IF NOT EXISTS(SELECT 1
        FROM information_schema.tables t
        WHERE t.table_schema = 'im'
          AND t.table_name = 'form_builder_form_tabs_fields_settings')
    THEN
        CREATE TABLE im.form_builder_form_tabs_fields_settings (
            id BIGINT NOT NULL DEFAULT(nextval('form_builder_form_tabs_fields_settings_id')),
            user_id UUID NOT NULL,
            field_id UUID NOT NULL,
            width INTEGER NOT NULL,
            CONSTRAINT pk_form_builder_form_tabs_fields_settings
                PRIMARY KEY (id),
            CONSTRAINT fk_form_builder_form_tabs_fields_settings_user
                FOREIGN KEY (user_id)
                REFERENCES im.users (im_obj_id)
                ON DELETE CASCADE ,
            CONSTRAINT fk_form_builder_form_tabs_fields_settings_field
                FOREIGN KEY (field_id)
                REFERENCES im.form_builder_form_tabs_fields (id)
                ON DELETE CASCADE,
            CONSTRAINT uq_form_builder_form_tabs_fields_settings
                UNIQUE (user_id, field_id)
        );
    END IF;
END;
$$