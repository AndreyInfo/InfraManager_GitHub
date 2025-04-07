DO
$$
BEGIN

	IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting' AND column_name = 'default_workplace_id' AND data_type = 'uuid')) THEN
		ALTER TABLE im.it_asset_import_setting DROP CONSTRAINT fk_it_asset_import_setting_default_workplace,
		ALTER COLUMN default_workplace_id TYPE VARCHAR(50);
		ALTER TABLE im.it_asset_import_setting ALTER COLUMN default_workplace_id TYPE uuid USING default_workplace_id::uuid;
	END IF;
	
	
	
	IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting' AND column_name = 'workflow_id' AND data_type = 'character varying')) THEN
		ALTER TABLE im.it_asset_import_setting DROP CONSTRAINT fk_it_asset_import_setting_workflow;
		ALTER TABLE im.it_asset_import_setting ALTER COLUMN workflow_id TYPE VARCHAR(50);
	END IF;
	
END
$$;