DO
$$
BEGIN

	IF (NOT EXISTS (SELECT * FROM information_schema.table_constraints 
	WHERE table_schema = 'im' AND table_name = 'asset' AND constraint_type = 'PRIMARY KEY' AND constraint_name = 'pk_asset')) THEN

		ALTER TABLE im.asset ADD CONSTRAINT pk_asset PRIMARY KEY (id);

	END IF;
	
	
	IF EXISTS (SELECT * FROM information_schema.columns WHERE table_schema = 'im' AND table_name = 'asset' AND column_name = 'tstamp') THEN

		ALTER TABLE im.asset DROP COLUMN tstamp;

	END IF;

END
$$;