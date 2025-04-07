DO
$$
BEGIN
if not exists (select id from im.operation where id = 1326) then
	insert into im.operation (id, class_id, name, operation_name, description) values (1326, 704, 'MassiveIncident.BeOwner', 'Быть владельцем массового инцидента', 'Операция дает возможность пользователю быть назначенным в качестве "Владельца" для объектов Массовый инцидент.');
end if;	

if not exists (select id from im.operation where id = 1327) then
	insert into im.operation (id, class_id, name, operation_name, description) values (1327, 704, 'ChangeRequest.ViewAll', 'Вкладка Запросы на изменение.', 'Операция предоставляет доступ к списку "Все запросы на изменение".');
end if;	

if not exists (select id from im.operation where id = 1328) then
	insert into im.operation (id, class_id, name, operation_name, description) values (1328, 823, 'MassiveIncident.Clone', 'Создать по аналогии', 'Операция позволяет добавлять новый объект Массовый инцидент, поля которого будут идентичны полям Массового инцидента, взятого за основу.');
end if;	

if not exists (select id from im.operation where id = 1329) then
	insert into im.operation (id, class_id, name, operation_name, description) values (1329, 823, 'MassiveIncident.EditServiceFields', 'Редактировать служебные поля массового инцидента', 'Операция позволяет изменять даты: Зарегистрирован, Открыт, Закрыть до, Выполнен, но не дает возможность открывать свойства массового инцидента.');
end if;	

if not exists (select id from im.operation where id = 1330) then
	insert into im.operation (id, class_id, name, operation_name, description) values (1330, 823, 'MassiveIncident.ChangeOwner', 'Передать', 'Операция позволяет назначить массовому инциденту другого владельца.');
end if;	

if not exists (select id from im.operation where id = 1331) then
	insert into im.operation (id, class_id, name, operation_name, description) values (1331, 823, 'MassiveIncident.SeeInherited', 'Видеть массовые инциденты ИТ-сотрудников', 'Позволяет увидеть пользователю в списке массовых инцидентов («Мое рабочее место», «Все массовые инциденты») массовые инциденты, которые не завершены и где владельцем или исполнителем, или инициатором являются сотрудники из того же подразделения что и этот пользователь или из подразделений ниже по иерархии');
end if;	

insert into im.role_operation (operation_id, role_id)
	select t.id, '00000000-0000-0000-0000-000000000001'
	from im.operation t
	left join im.role_operation x on x.operation_id = t.id and x.role_id = '00000000-0000-0000-0000-000000000001'
	where (t.id between 1326 and 1331) and (x.operation_id is null);
END
$$