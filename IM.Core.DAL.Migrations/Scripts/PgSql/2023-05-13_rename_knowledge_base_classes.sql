DO $$
begin
	update class set name = 'Классификатор базы знаний' where class_id = 138;
	update class set name = 'Настройки базы знаний' where class_id = 140;

	update operation set description = 'Операция позволяет просматривать свойства объекта Классификатор базы знаний через форму свойств, но не позволяет изменять.' where id = 493;
	update operation set description = 'Операция дает возможность создавать новый объект Классификатор базы знаний, но не дает возможности просмотра и изменения объекта Классификатор базы знаний.' where id = 494;
	update operation set description = 'Операция дает возможность удалять объект Классификатор базы знаний.' where id = 495;
	update operation set description = 'Операция позволяет измененять поля объекта Классификатор базы знаний через форму свойств.' where id = 496;

	insert into im.operation (id, class_id, name, operation_name, description)
		values (1368, 138, 'KBArticleFolder.BeExpert', 'Быть экспертом БЗ', 'Операция дает возможность пользователю быть назначенным в качестве "Эксперта БЗ"')
		on conflict (id) do nothing;

	insert into im.role_operation (operation_id, role_id) values (1368, '00000000-0000-0000-0000-000000000001');

	delete from operation where id in(502, 503);

	update operation set description = 'Операция позволяет просматривать свойства объекта Настройки базы знаний через форму свойств, но не позволяет изменять.' where id = 501;
	update operation set description = 'Операция позволяет измененять поля объекта Настройки базы знаний через форму свойств.' where id = 504;

	if not exists(select * from im.role_operation where operation_id = 1368 and role_id = '00000000-0000-0000-0000-000000000001') then
		insert into im.role_operation (operation_id, role_id) values (1368, '00000000-0000-0000-0000-000000000001');
	end if;
end;
$$;
