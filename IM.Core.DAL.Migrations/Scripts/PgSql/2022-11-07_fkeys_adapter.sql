DO $$
BEGIN

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_adapter_type') THEN
        ALTER TABLE im.adapter
            ADD CONSTRAINT fk_adapter_type
            FOREIGN KEY (adapter_type_id)
            REFERENCES im.adapter_type (adapter_type_id)
            NOT VALID;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_adapter_terminal_device') THEN
        ALTER TABLE im.adapter
            ADD CONSTRAINT fk_adapter_terminal_device
            FOREIGN KEY (terminal_device_id)
            REFERENCES im.terminal_equipment_types (identificator)
            NOT VALID;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_adapter_network_device') THEN
        ALTER TABLE im.adapter
            ADD CONSTRAINT fk_adapter_network_device
            FOREIGN KEY (network_device_id)
            REFERENCES im.active_equipment_types (identificator)
            NOT VALID;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_adapter_room') THEN
        ALTER TABLE im.adapter
            ADD CONSTRAINT fk_adapter_room
            FOREIGN KEY (room_id)
            REFERENCES im.room (identificator)
            NOT VALID;
    END IF;


    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_adapter_slot_type') THEN
        ALTER TABLE im.adapter
            ADD CONSTRAINT fk_adapter_slot_type
            FOREIGN KEY (slot_type_id)
            REFERENCES im.slot_type (identificator)
            NOT VALID;
    END IF;

END;
$$;