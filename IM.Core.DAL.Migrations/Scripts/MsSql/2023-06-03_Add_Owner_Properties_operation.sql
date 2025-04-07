if not exists (select * from dbo.Operation where ClassID = 29 and ID = 1)
begin
	INSERT INTO dbo.Operation values(1, 29, 'Owner.Update ', 'Открыть свойства', 'Операция позволяет просматривать название объекта Владелец через форму свойств, но не позволяет изменять его')
end;


if not exists (select * from dbo.RoleOperation where RoleID = '00000000-0000-0000-0000-000000000001' and OperationID = 1)
begin
    insert into dbo.RoleOperation values ('00000000-0000-0000-0000-000000000001', 1)
end;