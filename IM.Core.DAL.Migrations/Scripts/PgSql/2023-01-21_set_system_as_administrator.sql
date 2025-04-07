DO 
 $$
 BEGIN
    IF NOT EXISTS(select * from user_role 
                             where user_id = '00000000-0000-0000-0000-000000000000'
                             and role_id = '00000000-0000-0000-0000-000000000001')
    THEN
        insert into user_role(role_id, user_id)
        values ('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000000');
    END IF;
END
  $$;