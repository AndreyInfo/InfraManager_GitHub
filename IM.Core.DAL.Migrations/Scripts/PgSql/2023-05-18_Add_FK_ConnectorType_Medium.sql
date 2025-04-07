DO
$$
BEGIN

	IF (NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_connector_type_medium')) THEN
		ALTER TABLE im.connector_kinds 
		ADD CONSTRAINT fk_connector_type_medium FOREIGN KEY (medium_id)
		REFERENCES im.transmission_media_kinds (identificator);
	END IF;
END
$$;