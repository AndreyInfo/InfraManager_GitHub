ALTER TABLE IF EXISTS im.service_attendance
    ALTER COLUMN id SET DEFAULT gen_random_uuid();