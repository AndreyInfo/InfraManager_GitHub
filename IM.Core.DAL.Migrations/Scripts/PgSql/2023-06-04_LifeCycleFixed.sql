Update life_cycle
SET fixed = true
where id = '00000000-0000-0000-0000-000000000020';


ALTER TABLE IF EXISTS im.life_cycle_state
    ALTER COLUMN id SET DEFAULT gen_random_uuid();