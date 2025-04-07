INSERT into operation
    values(1, 29, 'Owner.Update ', 'Открыть свойства', 'Операция позволяет просматривать название объекта Владелец через форму свойств, но не позволяет изменять его')
ON CONFLICT DO NOTHING;

insert into im.role_operation values ('00000000-0000-0000-0000-000000000001', 1)
ON CONFLICT DO NOTHING;