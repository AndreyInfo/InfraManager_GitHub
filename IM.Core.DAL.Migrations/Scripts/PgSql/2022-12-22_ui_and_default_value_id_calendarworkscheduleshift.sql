CREATE UNIQUE INDEX if not exists ui_calendar_work_schedule_number_shift 
    on calendar_work_schedule_shift(calendar_work_schedule_id, number);

ALTER TABLE IF EXISTS im.calendar_work_schedule_shift
    ALTER COLUMN id SET DEFAULT (gen_random_uuid());