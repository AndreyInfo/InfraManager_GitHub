DO $$
BEGIN
    
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_peripheral_type') THEN
        ALTER TABLE im.peripheral
            ADD CONSTRAINT fk_peripheral_type
            FOREIGN KEY (peripheral_type_id)
            REFERENCES im.peripheral_type (peripheral_type_id)
            NOT VALID;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_peripheral_terminal_device') THEN
        ALTER TABLE im.peripheral
            ADD CONSTRAINT fk_peripheral_terminal_device
            FOREIGN KEY (terminal_device_id)
            REFERENCES im.terminal_equipment (identificator)
            NOT VALID;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_peripheral_network_device') THEN
        ALTER TABLE im.peripheral
            ADD CONSTRAINT fk_peripheral_network_device
            FOREIGN KEY (network_device_id)
            REFERENCES im.active_equipment (identificator)
            NOT VALID;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_peripheral_room') THEN
        ALTER TABLE im.peripheral
            ADD CONSTRAINT fk_peripheral_room
            FOREIGN KEY (room_id)
            REFERENCES im.room (identificator)
            NOT VALID;
    END IF;

END;
$$;