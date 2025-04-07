ALTER TABLE im.schedule ADD COLUMN IF NOT EXISTS next_at timestamp without time zone;
ALTER TABLE im.schedule ADD COLUMN IF NOT EXISTS last_at timestamp without time zone;