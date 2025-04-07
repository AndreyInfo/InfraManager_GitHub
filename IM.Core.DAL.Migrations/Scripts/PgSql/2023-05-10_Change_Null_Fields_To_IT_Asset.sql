DO
$$
BEGIN

	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting' AND column_name = 'it_asset_import_csv_configuration_id' AND IS_NULLABLE = 'NO') THEN

		ALTER TABLE im.it_asset_import_setting ALTER COLUMN it_asset_import_csv_configuration_id DROP NOT NULL;

	END IF;
	
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting' AND column_name = 'network_and_terminal_iden_param' AND IS_NULLABLE = 'YES') THEN

		ALTER TABLE im.it_asset_import_setting ALTER COLUMN network_and_terminal_iden_param SET NOT NULL;

	END IF;
	
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting' AND column_name = 'adapter_and_peripheral_iden_param' AND IS_NULLABLE = 'YES') THEN

		ALTER TABLE im.it_asset_import_setting ALTER COLUMN adapter_and_peripheral_iden_param SET NOT NULL;

	END IF;
	
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting' AND column_name = 'cu_iden_param' AND IS_NULLABLE = 'YES') THEN

		ALTER TABLE im.it_asset_import_setting ALTER COLUMN cu_iden_param SET NOT NULL;

	END IF;

END
$$;