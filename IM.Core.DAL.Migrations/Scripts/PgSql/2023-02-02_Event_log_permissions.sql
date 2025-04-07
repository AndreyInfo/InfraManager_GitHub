INSERT into im.operation
           values(1325, 906, 'Events.Properties ', 'Открыть свойства', 'Операция позволяет просматривать свойства объекта Журнал событий через форму свойств, но не позволяет изменять.')
ON CONFLICT DO NOTHING;


INSERT into im.Class values(906, 'Журнал событий')
ON CONFLICT DO NOTHING;


insert into im.role_operation values('00000000-0000-0000-0000-000000000001',1325)
ON CONFLICT DO NOTHING
