INSERT into im.Class values(911, 'Документальное оформление операций')
ON CONFLICT DO NOTHING;

INSERT into operation
    values(1381, 911, 'OperationalDocumentation.Update', 'Сохранить', 'Операция позволяет изменять поля на вкладке Документальное оформление операций через форму свойств.'),
		  (1380, 911, 'OperationalDocumentation.Properties ', 'Открыть свойства', 'Операция позволяет просматривать поля на вкладке Документальное оформление операций через форму свойств, но не позволяет изменять их.')
ON CONFLICT DO NOTHING;

insert into im.role_operation values ('00000000-0000-0000-0000-000000000001', 1380),
									 ('00000000-0000-0000-0000-000000000001', 1381)
ON CONFLICT DO NOTHING;