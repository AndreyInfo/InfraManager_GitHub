DO
$$
BEGIN

	INSERT into im.Class values (193, 'Задача импорта имущества')
		ON CONFLICT DO NOTHING;

	INSERT INTO im.operation VALUES (1356, 193, 'ITAssetImportSetting.Add', 'Создать', 'Операция дает возможность создавать новый объект Задача импорта имущества, но не дает возможности просмотра и изменения объекта.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1357, 193, 'ITAssetImportSetting.Update', 'Сохранить', 'Операция дает возможность изменять поля объекта Задача импорта имущества через форму свойств.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1358, 193, 'ITAssetImportSetting.Delete', 'Удалить', 'Операция дает возможность удалять объект Задача импорта имущества.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1359, 193, 'ITAssetImportSetting.Properties', 'Открыть свойства', 'Операция дает возможность просматривать поля объекта Задача импорта имущества через форму свойств, но не позволяет изменять их. На форме свойств объекта отсутствует кнопка "Сохранить".')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1360, 193, 'ITAssetImportSetting.AddAs', 'Создать по аналогии', 'Операция дает возможность создавать по аналогии новый объект Задачу импорта имущества, но не дает возможности просмотра и изменения объекта.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1361, 193, 'ITAssetImportSetting.Plan', 'Запланировать', 'Операция дает возможность запланировать Задачу импорта имущества из списка задач импорта имущества. Дает возможность сделать активным пункт меню "Запланировать" в списке задач импорта портфеля сервисов.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1362, 193, 'ITAssetImportSetting.Execute', 'Выполнить', 'Операция дает возможность запускать Задачу импорта имущества из списка задач импорта имущества. Дает возможность сделать активным пункт меню "Выполнить" в списке задач импорта портфеля сервисов.')
		ON Conflict DO Nothing;

	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1356) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1356)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1357) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1357)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1358) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1358)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1359) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1359)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1360) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1360)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1361) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1361)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1362) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1362)
	  ON Conflict DO Nothing;
	END IF;
	
	INSERT into im.Class values (194, 'Конфигурация CSV задачи импорта имущества')
		ON CONFLICT DO NOTHING;
	

	INSERT INTO im.operation VALUES (1363, 194, 'ITAssetImportCSVConfiguration.Add', 'Создать', 'Операция дает возможность создавать новый объект Конфигурация CSV задачи импорта имущества, но не дает возможности просмотра и изменения объекта.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1364, 194, 'ITAssetImportCSVConfiguration.Update', 'Сохранить', 'Операция дает возможность изменять поля объекта Конфигурация CSV задачи импорта имущества через форму свойств.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1365, 194, 'ITAssetImportCSVConfiguration.Delete', 'Удалить', 'Операция дает возможность удалять объект Конфигурация CSV задачи импорта имущества.')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1366, 194, 'ITAssetImportCSVConfiguration.Properties', 'Открыть свойства', 'Операция дает возможность просматривать поля объекта Конфигурация CSV задачи импорта имущества через форму свойств, но не позволяет изменять их. На форме свойств объекта отсутствует кнопка "Сохранить".')
		ON Conflict DO Nothing;

	INSERT INTO im.operation VALUES (1367, 194, 'ITAssetImportCSVConfiguration.AddAs', 'Создать по аналогии', 'Операция дает возможность создавать по аналогии новый объект Конфигурация CSV задачи импорта имущества, но не дает возможности просмотра и изменения объекта.')
		ON Conflict DO Nothing;

	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1363) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1363)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1364) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1364)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1365) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1365)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1366) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1366)
	  ON Conflict DO Nothing;
	END IF;
	
	IF NOT EXISTS (
	  SELECT FROM im.role_operation
	  WHERE  im.role_operation.role_id = '00000000-0000-0000-0000-000000000001' AND im.role_operation.operation_id = 1367) 
	THEN
	  INSERT INTO im.role_operation VALUES ('00000000-0000-0000-0000-000000000001', 1367)
	  ON Conflict DO Nothing;
	END IF;

END
$$;