CREATE TABLE IF NOT EXISTS im.form_builder_form
(
    id uuid NOT NULL,
    fields_is_required boolean NOT NULL,
    identifier text COLLATE pg_catalog."default" NOT NULL,
    class_id integer NOT NULL,
    name text COLLATE pg_catalog."default" NOT NULL,
    width integer NOT NULL,
    height integer NOT NULL,
    minor_version integer NOT NULL,
    major_version integer NOT NULL,
    description text COLLATE pg_catalog."default",
    status integer NOT NULL,
    last_index double precision NOT NULL,
    utc_changed date NOT NULL,
    product_type_id uuid,
    object_id uuid NOT NULL,
    CONSTRAINT form_builder_form_pkey PRIMARY KEY (id)
);


CREATE TABLE IF NOT EXISTS im.form_builder_form_tabs
(
    id uuid NOT NULL,
    name text COLLATE pg_catalog."default" NOT NULL,
    type text COLLATE pg_catalog."default",
    icon text COLLATE pg_catalog."default" NOT NULL,
    "order" integer NOT NULL,
    workflow_activity_form_id uuid NOT NULL,
    model text COLLATE pg_catalog."default",
    identifier text COLLATE pg_catalog."default",
    CONSTRAINT form_builder_form_tabs_pkey PRIMARY KEY (id),
    CONSTRAINT fk_form_tabs_form FOREIGN KEY (workflow_activity_form_id)
        REFERENCES im.form_builder_form (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS im.form_builder_form_tabs_fields
(
    id uuid NOT NULL,
    model text COLLATE pg_catalog."default",
    identifier text COLLATE pg_catalog."default" NOT NULL,
    category_name text COLLATE pg_catalog."default" NOT NULL,
    "order" integer NOT NULL,
    value text COLLATE pg_catalog."default",
    workflow_activity_form_tab_id uuid NOT NULL,
    type text COLLATE pg_catalog."default" NOT NULL,
    special_fields text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT form_builder_form_tabs_fields_pkey PRIMARY KEY (id),
    CONSTRAINT fk_tabs_field_form_tab FOREIGN KEY (workflow_activity_form_tab_id)
        REFERENCES im.form_builder_form_tabs (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);
	
	
	




CREATE TABLE IF NOT EXISTS im.form_builder_field_options
(
    id uuid NOT NULL,
    constant text COLLATE pg_catalog."default",
    operation_id integer NOT NULL,
    action_id integer NOT NULL,
    parent_identifier text COLLATE pg_catalog."default" NOT NULL,
    row_version timestamp without time zone,
    workflow_activity_form_field_id uuid NOT NULL,
    CONSTRAINT form_builder_field_options_pkey PRIMARY KEY (id),
    CONSTRAINT fk_field_options_field FOREIGN KEY (workflow_activity_form_field_id)
        REFERENCES im.form_builder_form_tabs_fields (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);
