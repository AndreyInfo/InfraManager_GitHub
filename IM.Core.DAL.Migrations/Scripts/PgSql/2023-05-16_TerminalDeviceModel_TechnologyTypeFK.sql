DO $$
BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_terminal_equipment_types_technology_kinds') 
  THEN
        ALTER TABLE im.terminal_equipment_types
            ADD CONSTRAINT fk_terminal_equipment_types_technology_kinds
            FOREIGN KEY (tchnology_id)
            REFERENCES im.technology_kinds (identificator);
  END IF;
END $$;
