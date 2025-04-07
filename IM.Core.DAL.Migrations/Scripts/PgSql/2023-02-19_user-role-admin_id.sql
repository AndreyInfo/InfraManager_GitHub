do $$
begin
	IF NOT EXISTS (SELECT 1 
		FROM im.user_role 
		WHERE user_id='00000000-0000-0000-0000-000000000001' and role_id='00000000-0000-0000-0000-000000000001')
	then
		insert into im.user_role (role_id, user_id) values ('00000000-0000-0000-0000-000000000001','00000000-0000-0000-0000-000000000001');
	end if;	
end $$;