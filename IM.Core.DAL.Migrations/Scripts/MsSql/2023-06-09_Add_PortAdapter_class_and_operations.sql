if not exists (select * from dbo.Class where ClassID = 39)
	insert into dbo.Class values(39, 'Порт объекта адаптер');

if not exists (select * from operation where ClassID = 39)
	INSERT INTO operation
		values(830027, 39,'PotrAdapter.Add','Создать','Операция дает возможность создавать новый Порт объекта Адаптер, но не дает возможности просмотра и изменения Порта объекта Адаптер.'),
			  (830028, 39,'PotrAdapter.Update','Сохранить','Операция позволяет изменять поля Порта объекта Адаптер через форму свойств.'),
			  (830029, 39,'PotrAdapter.Delete','Удалить','Операция дает возможность удалять Порт объекта Адаптер.'),
			  (830026, 39,'PotrAdapter.Properties','Открыть свойства','Операция позволяет просматривать свойства Порта объекта Адаптер через форму свойств, но не позволяет изменять.');

if not exists (select * from dbo.RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001' and
														(OperationID = 830026
														OR OperationID = 830027
														OR OperationID = 830028
														OR OperationID = 830029))
	insert into dbo.RoleOperation values ('00000000-0000-0000-0000-000000000001',830026),
										('00000000-0000-0000-0000-000000000001',830027),
										('00000000-0000-0000-0000-000000000001',830028),
										('00000000-0000-0000-0000-000000000001',830029);