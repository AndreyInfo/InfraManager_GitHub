DO $$
BEGIN
	ALTER TABLE im.adapter
	DROP CONSTRAINT fk_adapter_terminal_device;

	ALTER TABLE im.adapter
	ADD CONSTRAINT fk_adapter_terminal_device FOREIGN KEY (terminal_device_id)
	REFERENCES im.terminal_equipment (identificator)
	MATCH SIMPLE
	ON UPDATE NO ACTION
	ON DELETE NO ACTION;
END
$$;