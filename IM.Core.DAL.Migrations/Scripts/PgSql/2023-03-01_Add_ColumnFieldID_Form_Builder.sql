DO $$
BEGIN
    alter table form_builder_form_tabs_fields add column if not exists column_field_id uuid null;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_column_field') THEN
        ALTER TABLE im.form_builder_form_tabs_fields
            ADD CONSTRAINT fk_column_field
            FOREIGN KEY (column_field_id)
            REFERENCES im.form_builder_form_tabs_fields (id);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_group_field') THEN
        ALTER TABLE im.form_builder_form_tabs_fields
            ADD CONSTRAINT fk_group_field
            FOREIGN KEY (group_field_id)
            REFERENCES im.form_builder_form_tabs_fields (id);
    END IF;
END;
$$;