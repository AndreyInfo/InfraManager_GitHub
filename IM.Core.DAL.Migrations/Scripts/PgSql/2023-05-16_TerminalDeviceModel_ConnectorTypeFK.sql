DO $$
BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_terminal_equipment_types_connector_kinds') 
  THEN
        ALTER TABLE im.terminal_equipment_types
            ADD CONSTRAINT fk_terminal_equipment_types_connector_kinds
            FOREIGN KEY (connector_id)
            REFERENCES im.connector_kinds (identificator);
  END IF;
END $$;
