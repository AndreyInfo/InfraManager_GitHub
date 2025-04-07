CREATE TABLE IF NOT EXISTS im.it_asset_import_csv_configuration
(
	id uuid NOT NULL default(gen_random_uuid()),
	name CHARACTER VARYING(250) NOT NULL,
	note CHARACTER VARYING(500) NULL,
	delimiter CHARACTER VARYING(1) NOT NULL,
	CONSTRAINT pk_it_asset_import_csv_configuration PRIMARY KEY (id),
	CONSTRAINT uk_it_asset_import_csv_configuration_name UNIQUE (name)
);

CREATE TABLE IF NOT EXISTS im.it_asset_import_csv_configuration_class_concordance
(
	it_asset_import_csv_configuration_id uuid NOT NULL,
	field CHARACTER VARYING(50) NOT NULL,
	expression CHARACTER VARYING(2048) NOT NULL,
	CONSTRAINT pk_it_asset_import_csv_configuration_class_concordance PRIMARY KEY (it_asset_import_csv_configuration_id, field),
	CONSTRAINT fk_it_asset_import_csv_configuration_class_concordance_import_csv FOREIGN KEY (it_asset_import_csv_configuration_id) REFERENCES im.it_asset_import_csv_configuration(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS im.it_asset_import_csv_configuration_field_concordance
(
	it_asset_import_csv_configuration_id uuid NOT NULL,
	field CHARACTER VARYING(50) NOT NULL,
	expression CHARACTER VARYING(2048) NOT NULL,
	CONSTRAINT pk_it_asset_import_csv_configuration_field_concordance PRIMARY KEY (it_asset_import_csv_configuration_id, field),
	CONSTRAINT fk_it_asset_import_csv_configuration_field_concordance_import_csv FOREIGN KEY (it_asset_import_csv_configuration_id) REFERENCES im.it_asset_import_csv_configuration(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS im.it_asset_import_setting
(
	id uuid NOT NULL default(gen_random_uuid()),
	name CHARACTER VARYING(250) NOT NULL,
	note CHARACTER VARYING(500) NULL,
	it_asset_import_csv_configuration_id uuid NOT NULL,
	path CHARACTER VARYING(500) NOT NULL,
	create_model_automatically boolean NOT NULL DEFAULT(FALSE),
	default_model_id uuid NULL,
	missing_model_in_source boolean NOT NULL DEFAULT(FALSE),
	missing_type_in_source boolean NOT NULL DEFAULT(FALSE),
	unsuccessful_attempt_to_create_automatically boolean NOT NULL DEFAULT(FALSE),
	add_to_workplace_of_user boolean NOT NULL DEFAULT(FALSE),
	default_workplace_id int NULL,
	create_deviation boolean NOT NULL DEFAULT(FALSE),	
	create_messages boolean NOT NULL DEFAULT(FALSE),	
	create_summary_messages boolean NOT NULL DEFAULT(FALSE),	
	workflow_id uuid NULL,	
	CONSTRAINT pk_it_asset_import_setting PRIMARY KEY (id),
	CONSTRAINT uk_it_asset_import_setting_name UNIQUE (name),
	CONSTRAINT fk_it_asset_import_setting_it_asset_import_csv FOREIGN KEY (it_asset_import_csv_configuration_id) REFERENCES im.it_asset_import_csv_configuration(id) ON DELETE CASCADE,
	CONSTRAINT fk_it_asset_import_setting_default_workplace FOREIGN KEY (default_workplace_id) REFERENCES im.workplace(identificator) ON DELETE SET NULL,
	CONSTRAINT fk_it_asset_import_setting_workflow FOREIGN KEY (workflow_id) REFERENCES im.workflow(id) ON DELETE SET NULL
);