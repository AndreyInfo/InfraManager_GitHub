ALTER TABLE IF EXISTS im.service
    ALTER COLUMN id SET DEFAULT gen_random_uuid();