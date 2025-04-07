DO $$
begin
	IF EXISTS (SELECT 1 
		FROM information_schema.columns 
		WHERE table_schema='im' AND table_name='calendar_work_schedule_default' 
		AND column_name='time_end')
	then
        ALTER TABLE calendar_work_schedule_default
            ALTER COLUMN time_end SET DEFAULT '1900-01-01T18:00:00';
		
		Update calendar_work_schedule_default  SET time_end = time_start + (time_span_in_minutes ||' minutes')::interval;
	end if;	
end $$