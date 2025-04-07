if not exists (select 1 from Operation where ID = 750015)
BEGIN
	INSERT INTO Operation 
	VALUES (750015, 11004, 'UserImportDBConfiguration.Add', 'Создать', 'Операция дает возможность создавать новый объект "Конфигурация импорта из базы данных", но не дает возможности просмотра и изменения объекта.');
END;
GO

if not exists (select 1 from Operation where ID = 750016)
BEGIN
	INSERT INTO operation 
	VALUES (750016, 11004, 'UserImportDBConfiguration.Update', 'Сохранить', 'Операция дает возможность изменять поля объекта "Конфигурация импорта из базы данных" через форму свойств.');
END;
GO

if not exists (select 1 from Operation where ID = 750017)
BEGIN
	INSERT INTO operation 
	VALUES (750017, 11004, 'UserImportDBConfiguration.Delete', 'Удалить', 'Операция дает возможность удалять объект "Конфигурация импорта из базы данных".');
END;
GO

if not exists (select 1 from Operation where ID = 750018)
BEGIN
	INSERT INTO operation 
	VALUES (750018, 11004, 'UserImportDBConfiguration.Properties', 'Открыть свойства', 'Операция дает возможность просматривать поля объекта "Конфигурация импорта из базы данных" через форму свойств, но не позволяет изменять их. На форме свойств объекта отсутствует кнопка "Сохранить".');
END;
GO

if not exists (select 1 from Operation where ID = 750019)
BEGIN
	INSERT INTO operation 
	VALUES (750019, 11004, 'UserImportDBConfiguration.AddAs', 'Создать по аналогии', 'Операция дает возможность создавать по аналогии новый объект "Конфигурация импорта из базы данных", но не дает возможности просмотра и изменения объекта.')
END;

if NOT EXISTS (select 1 from [RoleOperation] 
	where RoleID = '00000000-0000-0000-0000-000000000001'
		AND [OperationID] in (750015, 750016, 750017, 750018, 750019))
BEGIN 
	INSERT INTO [RoleOperation]
	VALUES ('00000000-0000-0000-0000-000000000001', 750015)
		, ('00000000-0000-0000-0000-000000000001', 750016)
        , ('00000000-0000-0000-0000-000000000001', 750017)
        , ('00000000-0000-0000-0000-000000000001', 750018)
        , ('00000000-0000-0000-0000-000000000001', 750019);
END;


UPDATE Operation 
SET description = 'Операция дает возможность изменять поля объекта "Конфигурация импорта из базы данных" через форму свойств'
where id = 308;


if not exists (select 1 from Class where ClassID = 11004)
BEGIN
	INSERT INTO Class 
	VALUES (11004, 'Конфигурация импорта из базы данных');
END;