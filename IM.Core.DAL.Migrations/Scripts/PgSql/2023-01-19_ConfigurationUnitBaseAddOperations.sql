DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM im.operation WHERE class_id = 450) THEN
		INSERT into im.operation values
			(997, 450, 'ConfigurationUnitBase.Add ', 'Создать', 'Операция дает возможность создавать новый объект Конфигурационная единица, но не дает возможности просмотра и изменения объекта Конфигурационная единица.'),
			(998, 450, 'ConfigurationUnitBase.Update', 'Сохранить', 'Операция позволяет изменять поля объекта Конфигурационная единица через форму свойств.'),
			(999, 450, 'ConfigurationUnitBase.Delete', 'Удалить', 'Операция дает возможность удалять объект Конфигурационная единица.'),
			(1000, 450, 'ConfigurationUnitBase.Properties', 'Открыть свойства', 'Операция позволяет просматривать свойства объекта Конфигурационная единица через форму свойств, но не позволяет изменять.');
	END IF;

	IF NOT EXISTS (SELECT 1 FROM im.class WHERE class_id = 450) THEN
		INSERT into im.class values(450, 'Конфигурационная единица');
	END IF;

	IF NOT EXISTS (SELECT 1 FROM im.role_operation WHERE role_id = '00000000-0000-0000-0000-000000000001' and 
														(operation_id = 997
														OR operation_id = 998
														OR operation_id = 999
														OR operation_id = 1000)) THEN
		INSERT into im.role_operation values ('00000000-0000-0000-0000-000000000001',997),
												 ('00000000-0000-0000-0000-000000000001',998),
												 ('00000000-0000-0000-0000-000000000001',999),
												 ('00000000-0000-0000-0000-000000000001',1000);
	END IF;
END
$$;