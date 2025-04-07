DO
 $$
BEGIN
  if NOT exists (select constraint_name 
                    from information_schema.table_constraints 
                    where table_name = 'technology_compatibility' and constraint_type = 'PRIMARY KEY') 
  then
    ALTER TABLE technology_compatibility
        ADD PRIMARY KEY (tech_id1, tech_id2);
  end if;

  IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_from_technology_compatibility') 
  THEN
        ALTER TABLE im.technology_compatibility
            ADD CONSTRAINT fk_from_technology_compatibility
            FOREIGN KEY (tech_id1)
            REFERENCES im.technology_kinds (identificator)
            ON DELETE CASCADE;
  END IF;
  
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_to_technology_compatibility') 
  THEN
        ALTER TABLE im.technology_compatibility
            ADD CONSTRAINT fk_to_technology_compatibility
            FOREIGN KEY (tech_id2)
            REFERENCES im.technology_kinds (identificator)
            ON DELETE CASCADE;
  END IF;
END $$;


