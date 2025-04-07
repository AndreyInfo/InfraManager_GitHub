if not exists (select * from dbo.Class where ClassID = 911)
	insert into dbo.Class values(911, 'Документальное оформление операций');

if not exists (select * from operation where ClassID = 911)
	INSERT INTO operation
		values(1380, 911,'OperationalDocumentation.Update', 'Сохранить', 'Операция позволяет изменять поля на вкладке Документальное оформление операций через форму свойств.'),
			  (1381, 911,'OperationalDocumentation.Properties', 'Открыть свойства', 'Операция позволяет просматривать поля на вкладке Документальное оформление операций через форму свойств, но не позволяет изменять их.');

if not exists (select * from dbo.RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001' and OperationID = 1380)
	insert into dbo.RoleOperation values ('00000000-0000-0000-0000-000000000001', 1380);

if not exists (select * from dbo.RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001' and OperationID = 1381)
	insert into dbo.RoleOperation values ('00000000-0000-0000-0000-000000000001', 1381);
