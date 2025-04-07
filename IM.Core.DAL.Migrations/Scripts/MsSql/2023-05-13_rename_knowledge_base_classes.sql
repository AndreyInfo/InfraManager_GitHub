update dbo.Class set name = 'Классификатор базы знаний' where ClassID = 138;
update dbo.Class set name = 'Настройки базы знаний' where ClassID = 140;

update dbo.Operation set Description = 'Операция позволяет просматривать свойства объекта Классификатор базы знаний через форму свойств, но не позволяет изменять.' where ID = 493;
update dbo.Operation set Description = 'Операция дает возможность создавать новый объект Классификатор базы знаний, но не дает возможности просмотра и изменения объекта Классификатор базы знаний.' where ID = 494;
update dbo.Operation set Description = 'Операция дает возможность удалять объект Классификатор базы знаний.' where id = 495;
update dbo.Operation set Description = 'Операция позволяет измененять поля объекта Классификатор базы знаний через форму свойств.' where ID = 496;

delete from dbo.Operation where ID in(502, 503);

update dbo.Operation set Description = 'Операция позволяет просматривать свойства объекта Настройки базы знаний через форму свойств, но не позволяет изменять.' where ID = 501;
update dbo.Operation set Description = 'Операция позволяет измененять поля объекта Настройки базы знаний через форму свойств.' where ID = 504;

if not exists(select * from dbo.Operation where ID = 1368) 
begin
insert into dbo.Operation (ID, ClassID, Name, OperationName, Description)
	values (1368, 138, 'KBArticleFolder.BeExpert', 'Быть экспертом БЗ', 'Операция дает возможность пользователю быть назначенным в качестве "Эксперта БЗ"');
end

if not exists(select * from dbo.RoleOperation where OperationID = 1368)
begin
	insert into dbo.RoleOperation (OperationID, RoleID) values (1368, '00000000-0000-0000-0000-000000000001');
end