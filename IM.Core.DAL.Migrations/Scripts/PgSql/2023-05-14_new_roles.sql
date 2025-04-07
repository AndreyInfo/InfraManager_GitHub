DO $$
BEGIN

if not exists(select * from life_cycle where id = '00000000-0000-0000-0000-000000000021') then
	insert into life_cycle Values ('00000000-0000-0000-0000-000000000021', 'Этапы жизни статьи базы знаний', true, false, 18, null);

	insert into life_cycle_state values('00000000-0000-0000-0001-900000000005', 'Черновик', '00000000-0000-0000-0000-000000000021', false, false, true, false, false, false, false, false);
	insert into life_cycle_state values('00000000-0000-0000-0001-900000000006', 'Архив', '00000000-0000-0000-0000-000000000021', false, true, false, false, false, false, false, false);
	insert into life_cycle_state values('00000000-0000-0000-0001-900000000007', 'Опубликована', '00000000-0000-0000-0000-000000000021', false, false, false, false, false, false, false, true);

	insert into life_cycle_state_operation values('00000000-0000-0000-0005-500000000005', 'Обновление', 1, 10, null, '00000000-0000-0000-0001-900000000007', null, null);
	insert into life_cycle_state_operation values('00000000-0000-0000-0005-500000000006', 'Архивация', 2, 10, null, '00000000-0000-0000-0001-900000000007', null, null);
	insert into life_cycle_state_operation values('00000000-0000-0000-0005-500000000007', 'Публикация', 3, 10, null, '00000000-0000-0000-0001-900000000005', null, null);

	insert into life_cycle_state_operation_transition values('00000000-0000-0000-0005-500000000005', '00000000-0000-0000-0005-500000000006', '00000000-0000-0000-0001-900000000006', 0);
	insert into life_cycle_state_operation_transition values('00000000-0000-0000-0005-500000000006', '00000000-0000-0000-0005-500000000007', '00000000-0000-0000-0001-900000000007', 0);
	insert into life_cycle_state_operation_transition values('00000000-0000-0000-0005-500000000007', '00000000-0000-0000-0005-500000000005', '00000000-0000-0000-0001-900000000007', 0);

end if;


if not exists(select * from role where name like 'Автор БЗ') then
	begin
		insert into role(Name, Note) values ('Автор БЗ','');
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Автор БЗ%'), 338);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Автор БЗ%'), 339);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Автор БЗ%'), 489);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Автор БЗ%'), 490);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Автор БЗ%'), 491);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Автор БЗ%'), 492);

		insert into role_life_cycle_state_operation values((select id from role where name like '%Автор БЗ%'), '00000000-0000-0000-0005-500000000007');
	end;
end if;

if not exists(select * from role where name like 'Эксперт БЗ') then
	begin
		insert into role(Name, Note) values ('Эксперт БЗ','');
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Эксперт БЗ%'), 338);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Эксперт БЗ%'), 339);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Эксперт БЗ%'), 489);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Эксперт БЗ%'), 490);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Эксперт БЗ%'), 491);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Эксперт БЗ%'), 492);


		insert into role_life_cycle_state_operation values((select id from role where name like '%Эксперт БЗ%'), '00000000-0000-0000-0005-500000000005');
		insert into role_life_cycle_state_operation values((select id from role where name like '%Эксперт БЗ%'), '00000000-0000-0000-0005-500000000006');
		insert into role_life_cycle_state_operation values((select id from role where name like '%Эксперт БЗ%'), '00000000-0000-0000-0005-500000000007');
	end;
end if;

if not exists(select * from role where name like 'Менеджер БЗ') then
	begin
		insert into role(Name, Note) values ('Менеджер БЗ','');
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 338);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 339);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 489);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 490);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 491);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 492);

		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 495);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 496);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 493);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 494);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 1368);

		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 501);
		insert into role_operation(role_id, operation_id) values ((select id from role where name like '%Менеджер БЗ%'), 504);
	end;
end if;

if not exists(select * from setting where id = 134) then
	insert into setting values (134, decode('00000014', 'hex'));
end if;

if not exists(select * from setting where id = 135) then
	insert into setting values (135, decode('30303030303030302d303030302d303030302d303030302d303030303030303030303231', 'hex'));
end if;

if exists(select * from operation where id = 1327) then
	delete from operation where id = 1327;
end if;

end
$$
