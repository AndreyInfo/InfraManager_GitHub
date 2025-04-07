DO $$
BEGIN 

    ALTER TABLE IF EXISTS im.object_access
        ADD COLUMN IF NOT EXISTS id uuid DEFAULT gen_random_uuid();
    
    if NOT exists ( select * from information_schema.table_constraints 
                    where table_name = 'object_access'
                    and constraint_type = 'PRIMARY KEY') then
        ALTER TABLE object_access
            ADD PRIMARY KEY (id);
    end if;
END
$$

--select * from 