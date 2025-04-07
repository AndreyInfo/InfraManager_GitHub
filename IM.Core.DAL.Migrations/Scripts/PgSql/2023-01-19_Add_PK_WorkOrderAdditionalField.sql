DO
 $$
BEGIN
  if NOT exists (select constraint_name 
                    from information_schema.table_constraints 
                    where table_name = 'work_order_additional_field' and constraint_type = 'PRIMARY KEY') 
  then
    ALTER TABLE work_order_additional_field
        ADD PRIMARY KEY (id);
end if;
END $$;