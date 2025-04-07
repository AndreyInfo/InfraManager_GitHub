INSERT INTO operation
VALUES (750015, 11004, 'UserImportDBConfiguration.Add', 'Создать', 'Операция дает возможность создавать новый объект "Конфигурация импорта из базы данных", но не дает возможности просмотра и изменения объекта.')
    ON CONFLICT DO NOTHING;

INSERT INTO operation
VALUES (750016, 11004, 'UserImportDBConfiguration.Update', 'Сохранить', 'Операция дает возможность изменять поля объекта "Конфигурация импорта из базы данных" через форму свойств.')
    ON CONFLICT DO NOTHING;

INSERT INTO operation
VALUES (750017, 11004, 'UserImportDBConfiguration.Delete', 'Удалить', 'Операция дает возможность удалять объект "Конфигурация импорта из базы данных".')
    ON CONFLICT DO NOTHING;

INSERT INTO operation
VALUES (750018, 11004, 'UserImportDBConfiguration.Properties', 'Открыть свойства', 'Операция дает возможность просматривать поля объекта "Конфигурация импорта из базы данных" через форму свойств, но не позволяет изменять их. На форме свойств объекта отсутствует кнопка "Сохранить".')
    ON CONFLICT DO NOTHING;

INSERT INTO operation
VALUES (750019, 11004, 'UserImportDBConfiguration.AddAs', 'Создать по аналогии', 'Операция дает возможность создавать по аналогии новый объект "Конфигурация импорта из базы данных", но не дает возможности просмотра и изменения объекта.')
    ON CONFLICT DO NOTHING;

INSERT INTO role_operation
VALUES ('00000000-0000-0000-0000-000000000001', 750015)
     , ('00000000-0000-0000-0000-000000000001', 750016)
     , ('00000000-0000-0000-0000-000000000001', 750017)
     , ('00000000-0000-0000-0000-000000000001', 750018)
     , ('00000000-0000-0000-0000-000000000001', 750019)
    ON CONFLICT DO NOTHING;

UPDATE operation
SET description = 'Операция дает возможность изменять поля объекта "Конфигурация импорта из базы данных" через форму свойств'
where id = 308;

INSERT INTO class
VALUES (11004, 'Конфигурация импорта из базы данных')
    ON CONFLICT DO NOTHING;