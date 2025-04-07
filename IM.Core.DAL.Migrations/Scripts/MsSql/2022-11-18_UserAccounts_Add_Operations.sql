if not exists (select * from operation where ClassID = 902)
	INSERT INTO operation
		values(1102, 902, 'UserAccount.Add ', 'Создать', 'Операция дает возможность создавать новый объект Учетная запись, но не дает возможности просмотра и изменения объекта Учетная запись.'),
			  (1103, 902, 'UserAccount.Update', 'Сохранить', 'Операция позволяет изменять поля объекта Учетная запись через форму свойств.'),
			  (1104, 902, 'UserAccount.Delete', 'Удалить', 'Операция дает возможность удалять объект Учетная запись.'),
			  (1105, 902, 'UserAccount.Properties', 'Открыть свойства', 'Операция позволяет просматривать свойства объекта Учетная запись через форму свойств, но не позволяет изменять.')

if not exists (select * from dbo.Class where ClassID = 902)
	insert into dbo.Class values(902, 'Учетная запись')

if not exists (select * from dbo.RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001' and 
														(OperationID = 1102
														OR OperationID = 1103
														OR OperationID = 1104
														OR OperationID = 1105))
	insert into dbo.RoleOperation values ('00000000-0000-0000-0000-000000000001',1102),
										('00000000-0000-0000-0000-000000000001',1103),
										('00000000-0000-0000-0000-000000000001',1104),
										('00000000-0000-0000-0000-000000000001',1105)