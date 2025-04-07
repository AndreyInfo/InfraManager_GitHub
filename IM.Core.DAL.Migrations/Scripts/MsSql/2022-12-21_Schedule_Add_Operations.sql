if not exists (select * from operation where ClassID = 904)
	INSERT INTO operation
		values(1114, 904,'ScheduleTask_Open','Открыть свойства','Операция позволяет просматривать свойства объекта Задание планировщика, но не позволяет изменять.'),
			  (1115, 904,'ScheduleTask_Create','Создать','Операция позволяет создать объект Задание планировщика.'),
			  (1116, 904,'ScheduleTask_Save','Сохранить','Операция позволяет изменять объект Задание планировщика.'),
			  (1117, 904,'ScheduleTask_Stop','Остановить','Операция позволяет остановить объект Задание планировщика.'),
			  (1118, 904,'ScheduleTask_Delete','Удалить','Операция позволяет удалить объект Задание планировщика.')

if not exists (select * from dbo.Class where ClassID = 904)
	insert into dbo.Class values(904, 'Задача планировщика')

if not exists (select * from dbo.RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001' and
														(OperationID = 1114
														OR OperationID = 1115
														OR OperationID = 1116
														OR OperationID = 1117
														OR OperationID = 1118))
	insert into dbo.RoleOperation values ('00000000-0000-0000-0000-000000000001',1114),
										('00000000-0000-0000-0000-000000000001',1115),
										('00000000-0000-0000-0000-000000000001',1116),
										('00000000-0000-0000-0000-000000000001',1117),
										('00000000-0000-0000-0000-000000000001',1118)
