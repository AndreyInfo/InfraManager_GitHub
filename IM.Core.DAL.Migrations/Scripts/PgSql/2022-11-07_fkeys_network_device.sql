DO $$
BEGIN

    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_active_equipment_room') THEN
        ALTER TABLE im.active_equipment
            ADD CONSTRAINT fk_active_equipment_room
            FOREIGN KEY (room_id)
            REFERENCES im.room (identificator)
            NOT VALID;
    END IF;

END;
$$;