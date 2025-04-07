DO
$$
BEGIN

	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'adapter' AND column_name = 'terminal_device_id' AND IS_NULLABLE = 'NO') THEN

		ALTER TABLE im.adapter ALTER COLUMN terminal_device_id DROP NOT NULL;
		ALTER TABLE im.adapter ALTER COLUMN terminal_device_id DROP DEFAULT;

	END IF;

	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'adapter' AND column_name = 'network_device_id' AND IS_NULLABLE = 'NO') THEN

		ALTER TABLE im.adapter ALTER COLUMN network_device_id DROP NOT NULL;
		ALTER TABLE im.adapter ALTER COLUMN network_device_id DROP DEFAULT;

	END IF;
	
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'peripheral' AND column_name = 'terminal_device_id' AND IS_NULLABLE = 'NO') THEN

		ALTER TABLE im.peripheral ALTER COLUMN terminal_device_id DROP NOT NULL;
		ALTER TABLE im.peripheral ALTER COLUMN terminal_device_id DROP DEFAULT;

	END IF;

	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'peripheral' AND column_name = 'network_device_id' AND IS_NULLABLE = 'NO') THEN

		ALTER TABLE im.peripheral ALTER COLUMN network_device_id DROP NOT NULL;
		ALTER TABLE im.peripheral ALTER COLUMN network_device_id DROP DEFAULT;

	END IF;
	
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'active_equipment' AND column_name = 'logical_location' AND IS_NULLABLE = 'NO') THEN

		ALTER TABLE im.active_equipment ALTER COLUMN logical_location DROP NOT NULL;

	END IF;
	
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'active_equipment' AND column_name = 'description' AND IS_NULLABLE = 'NO') THEN

		ALTER TABLE im.active_equipment ALTER COLUMN description DROP NOT NULL;

	END IF;
	
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'active_equipment' AND column_name = 'identifier' AND IS_NULLABLE = 'NO') THEN

		ALTER TABLE im.active_equipment ALTER COLUMN identifier DROP NOT NULL;

	END IF;
	
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'terminal_equipment' AND column_name = 'logical_location' AND IS_NULLABLE = 'NO') THEN

		ALTER TABLE im.terminal_equipment ALTER COLUMN logical_location DROP NOT NULL;

	END IF;
	
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'terminal_equipment' AND column_name = 'description' AND IS_NULLABLE = 'NO') THEN

		ALTER TABLE im.terminal_equipment ALTER COLUMN description DROP NOT NULL;

	END IF;
	
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'im' AND table_name = 'terminal_equipment' AND column_name = 'identifier' AND IS_NULLABLE = 'NO') THEN

		ALTER TABLE im.terminal_equipment ALTER COLUMN identifier DROP NOT NULL;

	END IF;
	
	IF EXISTS (SELECT * FROM information_schema.columns WHERE table_schema = 'im' AND table_name = 'active_equipment' AND column_name = 'tstamp') THEN

		ALTER TABLE im.active_equipment DROP COLUMN tstamp;

	END IF;

END
$$;