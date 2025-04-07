if exists (select 1 from operation where ID = 820005)
BEGIN
	Update operation SET Description = N'Операция дает возможность создавать новый объект Причина отклонения от графика, но не дает возможности просмотра и изменения объекта Причина отклонения от графика '
	where id = 820005 
END;

if exists (select 1 from operation where ID = 820006)
BEGIN
	Update operation SET Description = N'Операция позволяет изменять поля объекта Причина отклонения от графика  через форму свойств, а также изменять положение в справочнике.',
					     OperationName = N'Сохранить'
	where ID = 820006
END;


if exists (select 1 from operation where ID = 820007)
BEGIN
	Update operation SET Description = N'Операция дает возможность удалять объект Причина отклонения от графика.'
	where id = 820007 
END;

if exists (select 1 from operation where ID = 820008)
BEGIN
	Update operation SET Description = N'Операция позволяет просматривать свойства объекта Причина отклонения через форму свойств, но не позволяет изменять.'
	where id = 820008 
END;