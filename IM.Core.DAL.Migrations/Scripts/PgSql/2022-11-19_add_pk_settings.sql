DO
 $$
BEGIN
  if NOT exists (select constraint_name 
                    from information_schema.table_constraints 
                    where table_name = 'setting' and constraint_type = 'PRIMARY KEY') 
  then
    ALTER TABLE setting
        ADD PRIMARY KEY (id);
end if;
END $$;