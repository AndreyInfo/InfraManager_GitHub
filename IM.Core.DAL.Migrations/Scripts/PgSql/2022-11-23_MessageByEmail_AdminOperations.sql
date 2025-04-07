DO $$begin
	if not exists(select from im.role_operation where role_id='00000000-0000-0000-0000-000000000001' and operation_id = 735001) then
		insert into role_operation values ('00000000-0000-0000-0000-000000000001', 735001);
	end if;
	if not exists(select from im.role_operation where role_id='00000000-0000-0000-0000-000000000001' and operation_id = 735002) then
		insert into role_operation values ('00000000-0000-0000-0000-000000000001', 735002);
	end if;
	if not exists(select from im.role_operation where role_id='00000000-0000-0000-0000-000000000001' and operation_id = 735003) then
		insert into role_operation values ('00000000-0000-0000-0000-000000000001', 735003);
	end if;
	if not exists(select from im.role_operation where role_id='00000000-0000-0000-0000-000000000001' and operation_id = 735004) then
		insert into role_operation values ('00000000-0000-0000-0000-000000000001', 735004);
	end if;
end$$;