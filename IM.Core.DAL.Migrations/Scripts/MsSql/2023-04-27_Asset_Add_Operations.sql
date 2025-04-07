if not exists (select * from operation where ClassID = 80)
	INSERT INTO operation
		values(993, 80,'Asset.Add','Создать','Операция дает возможность создавать новый объект Имущества, но не дает возможности просмотра и изменения объекта Имущества.'),
			  (994, 80,'Asset.Update','Сохранить','Операция позволяет изменять поля объекта Имущества через форму свойств.'),
			  (995, 80,'Asset.Delete','Удалить','Операция дает возможность удалять объект Имущество.'),
			  (996, 80,'Asset.Properties','Открыть свойства','Операция позволяет просматривать свойства объекта Имущества через форму свойств, но не позволяет изменять.')

if not exists (select * from dbo.Class where ClassID = 80)
	insert into dbo.Class values(80, 'Имущество')

if not exists (select * from dbo.RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001' and
														(OperationID = 993
														OR OperationID = 994
														OR OperationID = 995
														OR OperationID = 996))
	insert into dbo.RoleOperation values ('00000000-0000-0000-0000-000000000001',993),
										('00000000-0000-0000-0000-000000000001',994),
										('00000000-0000-0000-0000-000000000001',995),
										('00000000-0000-0000-0000-000000000001',996)
