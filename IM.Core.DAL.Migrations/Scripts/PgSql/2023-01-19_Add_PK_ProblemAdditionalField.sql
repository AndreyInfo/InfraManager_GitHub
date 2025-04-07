DO
 $$
BEGIN
  if NOT exists (select constraint_name 
                    from information_schema.table_constraints 
                    where table_name = 'problem_additional_field' and constraint_type = 'PRIMARY KEY') 
  then
    ALTER TABLE problem_additional_field
        ADD PRIMARY KEY (id);
end if;
END $$;