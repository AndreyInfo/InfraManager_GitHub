if not exists (select * from dbo.Class where ClassID = 26)
	insert into dbo.Class values(26, 'Шаблон слота');

if not exists (select * from operation where ClassID = 26)
	INSERT INTO operation
		values(1210, 26,'SlotTemplate.Add','Создать','Операция дает возможность создавать новый объект Шаблон слота, но не дает возможности просмотра и изменения объекта Шаблон слота.'),
			  (1211, 26,'SlotTemplate.Update','Сохранить','Операция позволяет изменять поля объекта Шаблон слота через форму свойств.'),
			  (1212, 26,'SlotTemplate.Delete','Удалить','Операция дает возможность удалять объект Шаблон слота.'),
			  (1213, 26,'SlotTemplate.Properties','Открыть свойства','Операция позволяет просматривать свойства объекта Шаблон слота через форму свойств, но не позволяет изменять.');

if not exists (select * from dbo.RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001' and
														(OperationID = 1210
														OR OperationID = 1211
														OR OperationID = 1212
														OR OperationID = 1213))
	insert into dbo.RoleOperation values ('00000000-0000-0000-0000-000000000001',1210),
										('00000000-0000-0000-0000-000000000001',1211),
										('00000000-0000-0000-0000-000000000001',1212),
										('00000000-0000-0000-0000-000000000001',1213);
