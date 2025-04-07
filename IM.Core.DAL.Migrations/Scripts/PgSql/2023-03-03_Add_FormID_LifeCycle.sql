ALTER TABLE IF EXISTS im.life_cycle
    ADD COLUMN IF NOT EXISTS form_id uuid;
	
DO
$$
BEGIN
    if not exists (SELECT 1 FROM information_schema.table_constraints WHERE constraint_name='fk_life_cycle_form_id' AND table_name='life_cycle')
    THEN
        ALTER TABLE life_cycle
            ADD CONSTRAINT fk_life_cycle_form_id 
			FOREIGN KEY (form_id)
            REFERENCES form_builder_form (id);
    end if;
end
$$