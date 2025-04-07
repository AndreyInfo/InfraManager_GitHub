DO $$
BEGIN
	ALTER TABLE im.adapter
	DROP CONSTRAINT fk_adapter_network_device;

	ALTER TABLE im.adapter
	ADD CONSTRAINT fk_adapter_network_device FOREIGN KEY (network_device_id)
	REFERENCES im.active_equipment (identificator)
	MATCH SIMPLE
	ON UPDATE NO ACTION
	ON DELETE NO ACTION;
END
$$;