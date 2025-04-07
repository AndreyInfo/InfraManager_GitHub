ALTER TABLE IF EXISTS im.life_cycle_state
    ALTER COLUMN id SET DEFAULT gen_random_uuid();