
if not exists(select * from LifeCycle where ID = '00000000-0000-0000-0000-000000000021')
	begin
		insert into LifeCycle(ID, Name, Fixed, Removed, Type, FormID) values ('00000000-0000-0000-0000-000000000021', 'Этапы жизни статьи базы знаний', 1, 0, 18, null);

		insert into LifeCycleState values('00000000-0000-0000-0001-900000000005', 'Черновик', '00000000-0000-0000-0000-000000000021', 0, 0, 1, 0, 0, 0, 0, 0);
		insert into LifeCycleState values('00000000-0000-0000-0001-900000000006', 'Архив', '00000000-0000-0000-0000-000000000021', 0, 1, 0, 0, 0, 0, 0, 0);
		insert into LifeCycleState values('00000000-0000-0000-0001-900000000007', 'Опубликована', '00000000-0000-0000-0000-000000000021', 0, 0, 0, 0, 0, 0, 0, 1);

		insert into LifeCycleStateOperation values('00000000-0000-0000-0005-500000000005', 'Обновление', 1, 10, null, '00000000-0000-0000-0001-900000000007', null, null);
		insert into LifeCycleStateOperation values('00000000-0000-0000-0005-500000000006', 'Архивация', 2, 10, null, '00000000-0000-0000-0001-900000000007', null, null);
		insert into LifeCycleStateOperation values('00000000-0000-0000-0005-500000000007', 'Публикация', 3, 10, null, '00000000-0000-0000-0001-900000000005', null, null);

		insert into LifeCycleStateOperationTransition values('00000000-0000-0000-0005-500000000005', '00000000-0000-0000-0005-500000000006', '00000000-0000-0000-0001-900000000006', 0);
		insert into LifeCycleStateOperationTransition values('00000000-0000-0000-0005-500000000006', '00000000-0000-0000-0005-500000000007', '00000000-0000-0000-0001-900000000007', 0);
		insert into LifeCycleStateOperationTransition values('00000000-0000-0000-0005-500000000007', '00000000-0000-0000-0005-500000000005', '00000000-0000-0000-0001-900000000007', 0);
	end;


if not exists(select * from role where name like 'Автор БЗ')
	begin
		insert into role(Name, Note) values ('Автор БЗ','');
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Автор БЗ%'), 338);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Автор БЗ%'), 339);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Автор БЗ%'), 489);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Автор БЗ%'), 490);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Автор БЗ%'), 491);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Автор БЗ%'), 492);

		insert into RoleLifeCycleStateOperation values((select ID from role where name like '%Автор БЗ%'), '00000000-0000-0000-0005-500000000007')
	end;

if not exists(select * from role where name like 'Эксперт БЗ')
	begin
		insert into role(Name, Note) values ('Эксперт БЗ','');
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Эксперт БЗ%'), 338);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Эксперт БЗ%'), 339);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Эксперт БЗ%'), 489);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Эксперт БЗ%'), 490);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Эксперт БЗ%'), 491);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Эксперт БЗ%'), 492);

		insert into RoleLifeCycleStateOperation values((select id from role where name like '%Эксперт БЗ%'), '00000000-0000-0000-0005-500000000005')
		insert into RoleLifeCycleStateOperation values((select id from role where name like '%Эксперт БЗ%'), '00000000-0000-0000-0005-500000000006')
		insert into RoleLifeCycleStateOperation values((select id from role where name like '%Эксперт БЗ%'), '00000000-0000-0000-0005-500000000007')
	end;

if not exists(select * from role where name like 'Менеджер БЗ')
	begin
		insert into role(Name, Note) values ('Менеджер БЗ','');
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 338);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 339);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 489);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 490);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 491);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 492);

		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 495);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 496);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 493);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 494);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 1368);

		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 501);
		insert into RoleOperation(RoleID, OperationID) values ((select id from role where name like '%Менеджер БЗ%'), 504);
	end;


if not exists(select * from dbo.Setting where id = 134)
	insert into dbo.Setting (id, Value) values(134, cast(20 as binary(4)))

if not exists(select * from dbo.Setting where id = 135)
	insert into dbo.Setting (id, Value) values(135, cast('00000000-0000-0000-0000-000000000021' as binary(36)))

if exists(select * from dbo.operation where id = 1327)
	delete from dbo.operation where id = 1327
