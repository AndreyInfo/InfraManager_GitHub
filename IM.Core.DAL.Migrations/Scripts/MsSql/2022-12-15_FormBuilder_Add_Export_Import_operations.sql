if not exists (select * from operation where ClassID = 903 and (ID = 1112 or ID = 1113))
	INSERT INTO operation
 		values(1112, 903, 'FormBuilder.Export ', 'Экспорт', 'Операция дает возможность экспортировать объект Форма.'),
			  (1113, 903, 'FormBuilder.Import', 'Импорт', 'Операция дает возможность импортировать объект Форма.')

if not exists (select * from dbo.Class where ClassID = 903)
	insert into dbo.Class values(903, 'Формы')

if not exists (select * from dbo.RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001' and 
														(OperationID = 1112
														OR OperationID = 1113))
	insert into dbo.RoleOperation values('00000000-0000-0000-0000-000000000001',1112),
										('00000000-0000-0000-0000-000000000001',1113)
