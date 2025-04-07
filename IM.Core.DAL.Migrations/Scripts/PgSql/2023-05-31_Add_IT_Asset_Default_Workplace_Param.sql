DO
$$
BEGIN

	IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting' AND column_name = 'default_location_not_specified_id')) THEN
		ALTER TABLE im.it_asset_import_setting ADD COLUMN default_location_not_specified_id BOOLEAN DEFAULT FALSE;
	END IF;

	IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting' AND column_name = 'default_location_not_found_id')) THEN
		ALTER TABLE im.it_asset_import_setting ADD COLUMN default_location_not_found_id BOOLEAN DEFAULT FALSE;
	END IF;

	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting' AND column_name = 'default_workplace_id') THEN
		ALTER TABLE im.it_asset_import_setting RENAME default_workplace_id TO default_location_id;
	END IF;

	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting' AND column_name = 'default_location_id' AND data_type = 'uuid') THEN
		ALTER TABLE im.it_asset_import_setting ALTER COLUMN default_location_id TYPE VARCHAR(50);
	END IF;

	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting' AND column_name = 'default_model_id' AND data_type = 'uuid') THEN
		ALTER TABLE im.it_asset_import_setting ALTER COLUMN default_model_id TYPE VARCHAR(50);
	END IF;
	
END
$$;