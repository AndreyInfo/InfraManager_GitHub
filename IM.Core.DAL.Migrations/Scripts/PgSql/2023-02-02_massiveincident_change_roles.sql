DO
$$
BEGIN
if exists (select id from im.operation where id = 980) then
	update im.operation set description = 'Операция позволяет просматривать свойства объекта Массовый инцидент через форму свойств, но не позволяет изменять.',
		operation_name = 'Открыть свойства' where id = 980;
end if;

if exists (select id from im.operation where id = 982) then
	update im.operation set description = 'Операция дает возможность создавать новый объект Массовый инцидент, но не дает возможности просмотра и изменения объекта Массовый инцидент.' 
	where id = 982;
end if;

if exists (select id from im.operation where id = 983) then
	update im.operation set description = 'Операция позволяет изменять поля объекта Массовый инцидент через форму свойств, а также его состояния.',
		operation_name = 'Сохранить' where id = 983; 
end if;

if exists (select id from im.operation where id = 984) then
	update im.operation set description = 'Операция дает возможность удалять объект Массовый инцидент.' 
	where id = 984;
end if;

if exists (select id from im.operation where id = 981) then
	update im.operation set description = 'Операция предоставляет доступ к списку "Все массовые инциденты"',
		operation_name = 'Вкладка Массовые инциденты.', class_id = 704 where id = 981; 
end if;		
END
$$