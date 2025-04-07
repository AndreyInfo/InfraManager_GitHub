ALTER TABLE IF EXISTS im.calendar_work_schedule_item
    ALTER COLUMN  shift_number DROP NOT NULL;

ALTER TABLE IF EXISTS im.calendar_work_schedule_item
    ADD COLUMN IF NOT EXISTS time_start timestamp(0) without time zone NOT NULL DEFAULT (now() at time zone 'utc');