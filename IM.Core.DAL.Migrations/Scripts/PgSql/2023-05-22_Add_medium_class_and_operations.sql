INSERT into im.Class values (79, 'Вид среды передачи')
ON CONFLICT DO NOTHING;

INSERT into operation
    values (97, 79, 'Medium.Properties', 'Открыть свойства', 'Операция позволяет просматривать свойства объекта Вид среды передачи через форму свойств, но не позволяет изменять.')
ON CONFLICT DO NOTHING;

INSERT into im.role_operation values ('00000000-0000-0000-0000-000000000001',97)
ON CONFLICT DO NOTHING;