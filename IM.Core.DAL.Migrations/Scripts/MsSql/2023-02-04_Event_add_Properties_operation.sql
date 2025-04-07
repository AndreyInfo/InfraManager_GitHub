if not exists (select * from operation where ClassID = 906)
	INSERT INTO operation
 		values(1325, 906, 'Events.Properties ', 'Открыть свойства', 'Операция позволяет просматривать свойства объекта Журнал событий через форму свойств, но не позволяет изменять.')
			  
if not exists (select * from dbo.Class where ClassID = 906)
	insert into dbo.Class values(906, 'Журнал событий')

if not exists (select * from dbo.RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001' and OperationID = 1325)
	insert into dbo.RoleOperation values('00000000-0000-0000-0000-000000000001',1325)