DO
$$
BEGIN

	IF (EXISTS (SELECT FROM information_schema.columns WHERE table_name = 'it_asset_import_setting')
	AND 'network_and_terminal_iden_param' NOT IN (SELECT column_name FROM information_schema.columns WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting')
	AND 'adapter_and_peripheral_iden_param' NOT IN (SELECT column_name FROM information_schema.columns WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting')
	AND 'cu_iden_param' NOT IN (SELECT column_name FROM information_schema.columns WHERE table_schema = 'im' AND table_name = 'it_asset_import_setting')) THEN
		ALTER TABLE im.it_asset_import_setting ADD COLUMN network_and_terminal_iden_param smallint;
		ALTER TABLE im.it_asset_import_setting ADD COLUMN adapter_and_peripheral_iden_param smallint;
		ALTER TABLE im.it_asset_import_setting ADD COLUMN cu_iden_param smallint;
	END IF;

END
$$;