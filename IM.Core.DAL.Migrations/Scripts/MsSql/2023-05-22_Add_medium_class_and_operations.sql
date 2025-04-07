if not exists (select * from dbo.Class where ClassID = 79)
	insert into dbo.Class values(79, 'Вид среды передачи');

if not exists (select * from operation where ClassID = 79)
	INSERT INTO operation
		values(97, 79,'Medium.Properties','Открыть свойства','Операция позволяет просматривать свойства объекта Вид среды передачи через форму свойств, но не позволяет изменять.');

if not exists (select * from dbo.RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001' and OperationID = 97)
	insert into dbo.RoleOperation values ('00000000-0000-0000-0000-000000000001',97);