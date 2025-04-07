INSERT INTO operation
VALUES (1206, 86, N'TelephoneType.Delete', N'Удалить', N'Операция дает возможность удалять объект Тип телефона.')
ON CONFLICT(id) DO UPDATE SET class_id = 86
					  , name = N'TelephoneType.Delete'
					  , operation_name = N'Удалить'
					  , description = N'Операция дает возможность удалять объект Тип телефона.'
                      