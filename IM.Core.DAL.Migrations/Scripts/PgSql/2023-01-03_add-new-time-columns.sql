do $$
begin
	IF EXISTS (SELECT 1 
		FROM information_schema.columns 
		WHERE table_schema='im' AND table_name='calendar_work_schedule_default' AND column_name='exclusion_time_start')
	then
		ALTER TABLE im.calendar_work_schedule_default
		RENAME COLUMN exclusion_time_start TO dinner_time_start;
	end if;	
end $$;


do $$
begin
	IF NOT EXISTS (SELECT 1 
		FROM information_schema.columns 
		WHERE table_schema='im' AND table_name='calendar_work_schedule_default' AND (column_name='dinner_time_end' OR column_name='time_end'))
	then
		ALTER TABLE im.calendar_work_schedule_default
		ADD COLUMN dinner_time_end TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT (now() AT TIME ZONE 'utc'),
		ADD COLUMN time_end TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL DEFAULT (now() AT TIME ZONE 'utc');
	end if;	
end $$;