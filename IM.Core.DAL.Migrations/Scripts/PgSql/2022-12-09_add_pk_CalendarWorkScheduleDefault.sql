DO
$$
BEGIN
    
    if not exists (SELECT column_name
                          FROM information_schema.columns
                          WHERE table_name='calendar_work_schedule_default' 
                                and column_name='id')
    then                      
        ALTER TABLE IF EXISTS im.calendar_work_schedule_default
            ADD COLUMN id uuid NOT NULL DEFAULT gen_random_uuid();
    end if;
    
  if NOT exists (select constraint_name 
                    from information_schema.table_constraints 
                    where table_name = 'calendar_work_schedule_default' and constraint_type = 'PRIMARY KEY') 
  then
    ALTER TABLE calendar_work_schedule_default
        ADD PRIMARY KEY (id);
    end if;
END $$;